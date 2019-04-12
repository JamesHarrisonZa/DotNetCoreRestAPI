using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Services.Models;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		[HttpPost]
		public ActionResult<DetailBooking> Post([FromBody] string text)
		{
            return new BookingService().GetDetailBooking(text);
        }
	}
}
