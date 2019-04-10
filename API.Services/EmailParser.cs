using System;
using API.Services.Models;

namespace API.Services
{
	public class EmailParser
	{
		// if (hasNonMatchingTags) return rejected message
		// if (missingTotal) return rejected message
		// if (missingCostCentre) then the field in the output should be defaulted to ‘UNKNOWN’. 

		public Booking GetBooking(string email)
		{
			var costCentre = "";
			var total = 0m;
			var paymentMethod = "";
			var vendor = "";
			var description = "";
			var date = DateTime.Now;
			return new Booking(costCentre, total, paymentMethod, vendor, description, date);
		}
	}
}
