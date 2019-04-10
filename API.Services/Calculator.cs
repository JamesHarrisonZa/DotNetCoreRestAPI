using System;

namespace API.Services
{
	public class Calculator
	{
		private decimal _gstPercentage = 15m;

		public decimal GetGst(decimal totalIncludingGst)
		{
			return totalIncludingGst * (_gstPercentage/100);
		}
	}
}
