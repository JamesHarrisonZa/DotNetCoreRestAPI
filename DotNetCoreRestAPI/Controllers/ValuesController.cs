using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreRestAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GstController : ControllerBase
	{
		[HttpPost]
		public ActionResult<string> Post([FromBody] string value)
		{
			//1: Extract XML content based on <tags>
			//2: Calculate the GST and total excluding GST. The extracted <total> includes GST
			//3: Return extracted + calculated info

			return value;
		}
	}
}
