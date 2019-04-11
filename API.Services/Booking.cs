using API.Services.Models;

namespace API.Services
{
    public class Booking
    {
        public DetailBooking GetDetailBooking(string email)
        {
            var emailParser = new EmailParser();
            var emailBooking = emailParser.GetEmailBooking(email);

            var calculator = new Calculator();
            var gst = calculator.GetGst(emailBooking.Total);
            var totalExcludingGst = calculator.GetTotalExcludingGst(emailBooking.Total, gst);

            return new DetailBooking(emailBooking, gst, totalExcludingGst);
        }
    }
}
