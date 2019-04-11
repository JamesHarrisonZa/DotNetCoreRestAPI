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
        // if (missingTotal) return rejected message
        // if (missingCostCentre) then the field in the output should be defaulted to ‘UNKNOWN’.
        // if (hasNonMatchingTags) return rejected message

        //ToDo: Probably make the tags configurable
        public EmailBooking GetEmailBooking()
		{
            var costCentre = GetCheckedCostCentre(GetBetweenTags(_email, "<cost_centre>", "</cost_centre>"));
            var total = Convert.ToDecimal(GetBetweenTags(_email, "<total>", "</total>"));
			var paymentMethod = GetBetweenTags(_email, "<payment_method>", "</payment_method>");
            var vendor = GetBetweenTags(_email, "<vendor>", "</vendor>");
            var description = GetBetweenTags(_email, "<description>", "</description>");
            var date = GetCheckedDate(GetBetweenTags(_email, "<date>", "</date>"));

            return new EmailBooking(costCentre, total, paymentMethod, vendor, description, date);
		}

        private string GetBetweenTags(string searchText, string openingTag, string closingTag)
        {
            var startIndex = searchText.IndexOf(openingTag);
            var endIndex = searchText.IndexOf(closingTag);

            if (IsMissingBothTags(startIndex, endIndex))
                return "";
            //ToDo: HasNonMatchingTags

            return searchText.Substring(startIndex + openingTag.Length, endIndex - (startIndex + openingTag.Length));
        }

        private string GetCheckedCostCentre(string costCentre)
        {
            if (string.IsNullOrEmpty(costCentre))
                return "UNKNOWN";
            return costCentre;
        }

        //My interpretation as the example contained an invalid date but wasnt a part of the failure conditions.
        private string GetCheckedDate(string date)
        {
            if (date == "")
                return "";

            DateTime result;
            if(DateTime.TryParse(date, out result))
                return date;
            return date + ", please double check this date as it may not be valid";
        }

        private bool IsMissingBothTags(int startIndex, int endIndex)
        {
            return startIndex == -1 && endIndex == -1;
        }
    }
}
