using System;

namespace API.Services
{
	public class Calculator
	{
		private decimal _gstPercentage = 15m; //ToDo: Potentially make configurable? Inject into constructor?

		public decimal GetGst(decimal totalIncludingGst)
		{
			return totalIncludingGst * (_gstPercentage/100);
		}

		public decimal GetTotalExcludingGst(decimal totalIncludingGst, decimal gst)
		{
			return totalIncludingGst - gst;
		}
	}
}
