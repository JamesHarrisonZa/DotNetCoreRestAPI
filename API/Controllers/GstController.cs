using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GstController : ControllerBase
	{
		[HttpPost]
		public ActionResult<string> Post([FromBody] string text)
		{
            // 1: Extract XML content based on <tags>
            var emailParser = new EmailParser();
            var booking = emailParser.GetBooking(text);
            // 2: Calculate the GST and total excluding GST. The extracted <total> includes GST. 
            var calculator = new Calculator();
            var gst = calculator.GetGst(booking.Total);
            var totalExcludingGst = calculator.GetTotalExcludingGst(booking.Total, gst);
            // 3: Return extracted + calculated info
            //ToDo: return json object with all the info
            return $"gst: {gst}, totalExcludingGst: {totalExcludingGst}";
		}
	}
}
