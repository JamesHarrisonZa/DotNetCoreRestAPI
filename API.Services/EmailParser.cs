using System;
using API.Services.Models;

namespace API.Services
{
	public class EmailParser
	{
        private string _email;

        public EmailParser(string email) {
            _email = email;
        }

        //ToDo:
        // if (hasNonMatchingTags) return rejected message
        // if (missingTotal) return rejected message
        // if (missingCostCentre) then the field in the output should be defaulted to ‘UNKNOWN’. 

        public EmailBooking GetEmailBooking()
		{
            var costCentre = GetBetweenTags(_email, "<cost_centre>", "</cost_centre>");
            var total = Convert.ToDecimal(GetBetweenTags(_email, "<total>", "</total>"));
			var paymentMethod = GetBetweenTags(_email, "<payment_method>", "</payment_method>");
            var vendor = GetBetweenTags(_email, "<vendor>", "</vendor>");
            var description = GetBetweenTags(_email, "<description>", "</description>");
            var date = GetCheckedDate(GetBetweenTags(_email, "<date>", "</date>"));

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
