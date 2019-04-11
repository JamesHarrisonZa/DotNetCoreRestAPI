using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Services.Exceptions;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		[HttpPost]
		public ActionResult<string> Post([FromBody] string text)
		{
            try
            {
                var detailBooking = new BookingService().GetDetailBooking(text);
                return JsonConvert.SerializeObject(detailBooking);
            }
            catch (InvalidMessageException ex)
            {
                return ex.Message;
            }
        }
	}
}
