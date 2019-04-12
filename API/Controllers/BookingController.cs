using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Services.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
		public ActionResult<DetailBooking> Post([FromBody] string text)
		{
            return _bookingService.GetDetailBooking(text);
        }
	}
}
