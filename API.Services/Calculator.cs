namespace API.Services
{
	public class Calculator : ICalculator
	{
		private const decimal _gstPercentage = 15m; //ToDo: Consider making configurable.

		public decimal GetGst(decimal totalIncludingGst)
		{
			return totalIncludingGst * (_gstPercentage / 100);
		}

		public decimal GetTotalExcludingGst(decimal totalIncludingGst, decimal gst)
		{
			return totalIncludingGst - gst;
		}
	}
}
