using API.Services.Models;

namespace API.Services
{
    public class BookingService
    {
        public DetailBooking GetDetailBooking(string email)
        {
            var emailParser = new EmailParser(email);
            var emailBooking = emailParser.GetEmailBooking();

            var calculator = new Calculator();
            var gst = calculator.GetGst(emailBooking.Total);
            var totalExcludingGst = calculator.GetTotalExcludingGst(emailBooking.Total, gst);

            return new DetailBooking(emailBooking, gst, totalExcludingGst);
        }
    }
}
