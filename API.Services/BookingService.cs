using API.Services.Models;

namespace API.Services
{
	public class BookingService : IBookingService
	{
		private readonly IEmailParser _emailParser;
		private readonly ICalculator _calculator;

		public BookingService(IEmailParser emailParser, ICalculator calculator)
		{
			_emailParser = emailParser;
			_calculator = calculator;
		}

		public DetailBooking GetDetailBooking(string email)
		{
			var emailBooking = _emailParser.GetEmailBooking(email);

			var gst = _calculator.GetGst(emailBooking.Total);
			var totalExcludingGst = _calculator.GetTotalExcludingGst(emailBooking.Total, gst);

			return new DetailBooking(emailBooking, gst, totalExcludingGst);
		}
	}
}
