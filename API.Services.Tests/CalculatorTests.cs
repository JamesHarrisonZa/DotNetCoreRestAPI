using NUnit.Framework;
using API.Services;

namespace Tests
{
	[TestFixture]
	public class CalculatorTests
	{
		[TestCase(1000, 150)]
		[TestCase(1024.01, 153.6015)]
		public void GetGst(decimal totalIncludingGst, decimal expected)
		{
			var calculator = CreateSut();
			var actual = calculator.GetGst(totalIncludingGst);
			Assert.AreEqual(expected, actual);
		}

		[TestCase(1000, 150, 850)]
		[TestCase(1024.01, 153.6015, 870.4085)]
		public void GetTotalExcludingGst(decimal totalIncludingGst, decimal gst, decimal expected)
		{
			var calculator = CreateSut();
			var actual = calculator.GetTotalExcludingGst(totalIncludingGst, gst);
			Assert.AreEqual(expected, actual);
		}

		private Calculator CreateSut()
		{
			return new Calculator();
		}
	}
}