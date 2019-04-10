using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace API.Services.Tests.Helpers
{
    //This is my go to equality comparer that ive worked on in other projects
    public class StructuralEqualityComparer : IEqualityComparer
    {
        private readonly Func<PropertyInfo, bool> _excludePredicate;
        private readonly StructuralEqualityComparerOptions _options;
        private readonly IDictionary<Type, Delegate> _equalsDelegateCache = new Dictionary<Type, Delegate>();

        public StructuralEqualityComparer(Func<PropertyInfo, bool> excludePredicate = null, StructuralEqualityComparerOptions options = null)
        {
            Func<PropertyInfo, bool> defaultExcludePredicate = (propertyInfo) => false;
            _excludePredicate = excludePredicate ?? defaultExcludePredicate;
            _options = options ?? new StructuralEqualityComparerOptions();
        }

        public StructuralEqualityComparer(IEnumerable<string> excludePropertyName)
            : this(propertyInfo => excludePropertyName.Contains(propertyInfo.Name))
        {
        }

        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
                return x == y;

            var xType = Nullable.GetUnderlyingType(x.GetType()) ?? x.GetType();
            var yType = Nullable.GetUnderlyingType(y.GetType()) ?? y.GetType();

            if (xType == typeof(decimal) || yType == typeof(decimal))
                return GenericEquals(x, y, typeof(decimal));

            if (xType == typeof(string) || yType == typeof(string))
                return GenericEquals(x, y, typeof(string));

            if (xType == typeof(DateTime) || yType == typeof(DateTime))
                return GenericEquals(x, y, typeof(DateTime));

            if ((x is IEnumerable) || y is IEnumerable)
            {
                return SequenceEquals(x as IEnumerable, y as IEnumerable, (xObj, yObj) =>
                {
                    return Equals(xObj, yObj);
                });
            }

            if (xType.IsPrimitive || yType.IsPrimitive)
                return GenericEquals(x, y, xType) || GenericEquals(y, x, yType);

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var xProperties = x.GetType().GetProperties(bindingFlags);
            var xPropertiesToCompare = xProperties
                .Except(xProperties.Where(_excludePredicate))
                .OrderBy(propertyInfo => propertyInfo.Name)
                .ToList();

            var yProperties = y.GetType().GetProperties(bindingFlags);
            var yPropertiesToCompare = yProperties
                .Except(yProperties.Where(_excludePredicate))
                .OrderBy(propertyInfo => propertyInfo.Name)
                .ToList();

            return SequenceEquals(xPropertiesToCompare, yPropertiesToCompare, (value1, value2) =>
            {
                return PropertyEquals(x, y, value1 as PropertyInfo, value2 as PropertyInfo);
            });
        }

        private bool SequenceEquals(IEnumerable first, IEnumerable second, Func<object, object, bool> equalityPredicate)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            {
                while (firstEnumerator.MoveNext())
                {
                    if (!secondEnumerator.MoveNext())
                        return false;
                    if (!equalityPredicate(firstEnumerator.Current, secondEnumerator.Current))
                        return false;
                }

                if (secondEnumerator.MoveNext())
                    return false;
            }

            return true;
        }

        public int GetHashCode(object obj) => obj.GetHashCode();

        private bool PropertyEquals(object x, object y, PropertyInfo xPropertyInfo, PropertyInfo yPropertyInfo)
        {
            if (xPropertyInfo == null)
                return yPropertyInfo == null;
            if (yPropertyInfo == null)
                return false;
            if (_options.NameCompare)
            {
                if (xPropertyInfo.Name != yPropertyInfo.Name)
                    return false;
            }
            if (_options.TwoWayAssignableCompare)
            {
                if (!xPropertyInfo.PropertyType.IsAssignableFrom(yPropertyInfo.PropertyType) || !yPropertyInfo.PropertyType.IsAssignableFrom(xPropertyInfo.PropertyType))
                    return false;
            }
            else if (_options.OneWayAssignableCompare)
            {
                if (!xPropertyInfo.PropertyType.IsAssignableFrom(yPropertyInfo.PropertyType))
                    return false;
            }
            if (_options.ValueCompare)
            {
                if (xPropertyInfo.GetIndexParameters().Length > 0)
                    throw new Exception("Index parameters types are not (yet) supported in StructuralEqualityComparer!");
                if (yPropertyInfo.GetIndexParameters().Length > 0)
                    throw new Exception("Index parameters types are not (yet) supported in StructuralEqualityComparer!");

                var xValue = xPropertyInfo.GetValue(x);
                var yValue = yPropertyInfo.GetValue(y);

                if (xValue == null || yValue == null)
                    return xValue == yValue;

                //if (xValue is IStructuralComparable)
                //    throw new Exception("IStructuralComparable is not (yet) supported in StructuralEqualityComparer!");
                //if (yValue is IStructuralComparable)
                //    throw new Exception("IStructuralComparable is not (yet) supported in StructuralEqualityComparer!");

                // Prevent self reference properties for infinite recursion
                if (xValue == x || yValue == y)
                    return true;
                if (xValue.GetType() == x.GetType() || yValue.GetType() == y.GetType())
                    return true;

                if (_options.RecursiveCompare)
                    return Equals(xValue, yValue);
            }
            return true;
        }

        private bool GenericEquals(object x, object y, Type type)
        {
            if (x.GetType() != type || y.GetType() != type)
                return false;

            if (!_equalsDelegateCache.ContainsKey(type))
            {
                var genericEqualityComparerType = typeof(EqualityComparer<>).MakeGenericType(type);
                var genericEqualityComparer = genericEqualityComparerType.GetProperty(nameof(EqualityComparer<object>.Default), BindingFlags.Public | BindingFlags.Static).GetValue(null);
                var equalsMethodInfo = genericEqualityComparerType.GetMethod(nameof(EqualityComparer<object>.Equals), BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                var delegateType = typeof(Func<,,>).MakeGenericType(type, type, typeof(bool));
                var equalsDelegate = equalsMethodInfo.CreateDelegate(delegateType, genericEqualityComparer);
                _equalsDelegateCache[type] = equalsDelegate;
            }
            return (bool)_equalsDelegateCache[type].DynamicInvoke(x, y);
        }
    }

    public class StructuralEqualityComparerOptions
    {
        /// <summary>
        /// Check that the names of the properties match
        /// </summary>
        public bool NameCompare { get; set; } = true;
        /// <summary>
        /// Check that the Type of TY can be assigned to TX.
        /// </summary>
        public bool OneWayAssignableCompare { get; set; } = true;
        /// <summary>
        /// Check that the Type of TY can be assigned to TX and vise versa, skips OneWayAssignableCompare.
        /// </summary>
        public bool TwoWayAssignableCompare { get; set; } = false;
        /// <summary>
        /// Check that the values of the properties match, using EqualityComparer
        /// </summary>
        public bool ValueCompare { get; set; } = true;
        /// <summary>
        /// Recursively walk for nested types
        /// </summary>
        public bool RecursiveCompare { get; set; } = true;
    }
}
