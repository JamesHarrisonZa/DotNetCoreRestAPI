using Microsoft.AspNetCore.Mvc;
using API.Services;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		[HttpPost]
		public ActionResult<string> Post([FromBody] string text)
		{
            var detailBooking = new Booking().GetDetailBooking(text);
            return JsonConvert.SerializeObject(detailBooking);
        }
	}
}
