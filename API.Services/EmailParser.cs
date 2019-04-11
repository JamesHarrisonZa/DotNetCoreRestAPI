using System;
using API.Services.Models;

namespace API.Services
{
	public class EmailParser
	{
        //ToDo:
		// if (hasNonMatchingTags) return rejected message
		// if (missingTotal) return rejected message
		// if (missingCostCentre) then the field in the output should be defaulted to ‘UNKNOWN’. 

		public EmailBooking GetEmailBooking(string email)
		{
            var costCentre = GetBetweenTags(email, "<cost_centre>", "</cost_centre>");
            var total = Convert.ToDecimal(GetBetweenTags(email, "<total>", "</total>"));
			var paymentMethod = GetBetweenTags(email, "<payment_method>", "</payment_method>");
            var vendor = GetBetweenTags(email, "<vendor>", "</vendor>");
            var description = GetBetweenTags(email, "<description>", "</description>");
            var date = GetCheckedDate(GetBetweenTags(email, "<date>", "</date>"));

            return new EmailBooking(costCentre, total, paymentMethod, vendor, description, date);
		}

        private string GetBetweenTags(string searchText, string openingTag, string closingTag)
        {
            var startIndex = searchText.IndexOf(openingTag) + openingTag.Length;
            var endIndex = searchText.IndexOf(closingTag);
            return searchText.Substring(startIndex, endIndex - startIndex);
        }

        //My interpretation as the example contained an invalid date but wasnt a part of the failure conditions.
        private string GetCheckedDate(string date)
        {
                DateTime result;
                if(DateTime.TryParse(date, out result))
                    return date;
                return date + ", please double check this date as it may not be valid";
        }
	}
}
