using NUnit.Framework;
using API.Services;

namespace Tests
{
	[TestFixture]
	public class CalculatorTests
	{
		[TestCase(1000, 150)]
		[TestCase(1024.01, 153.6015)]
		public void GetGst_Given_TotalIncludingGst_Should_Return_Gst(decimal totalIncludingGst, decimal expected)
		{
			var calculator = new Calculator();
			var actual = calculator.GetGst(totalIncludingGst);
			Assert.AreEqual(expected, actual);
		}
	}
}