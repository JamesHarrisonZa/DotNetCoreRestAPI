using NUnit.Framework;
using API.Services.Models;
using API.Services.Tests.Helpers;

namespace API.Services.Tests
{
    public class EmailParserTests
    {
        [TestCase(@"
		Hi Yvaine, 
		Please create an expense claim for the below.  Relevant details are marked up as requested… 
		
		<expense><cost_centre>DEV002</cost_centre> <total>1024.01</total><payment_method>personal card</payment_method> </expense> 
		
		From: Ivan Castle  Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com> Subject: test 
		
		Hi Antoine,   
		
		Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>.  We expect to arrive around 7.15pm.  Approximately 12 people but I’ll confirm exact numbers closer to the day. 
		
		Regards, Ivan"
        , "DEV002", 1024.01, "personal card", "Viaduct Steakhouse", "development team’s project end celebration dinner", "Tuesday 27 April 2017, please double check this date as it may not be valid")]
        [TestCase(@"
		Testing valid date
		<expense><cost_centre>DEV002</cost_centre> <total>1024.01</total><payment_method>personal card</payment_method> </expense> 
			
		Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Thursday 27 April 2017</date>.
		"
        , "DEV002", 1024.01, "personal card", "Viaduct Steakhouse", "development team’s project end celebration dinner", "Thursday 27 April 2017")]
        public void GetEmailBooking(string email, string costCentre, decimal total, string paymentMethod, string vendor, string description, string date)
        {
            var sut = CreateSut(email);
            var actual = sut.GetEmailBooking();
            var expected = new EmailBooking(costCentre, total, paymentMethod, vendor, description, date);

            var structuralEqualityComparer = new StructuralEqualityComparer();
            Assert.That(actual, Is.EqualTo(expected).Using(structuralEqualityComparer));
        }

        private EmailParser CreateSut(string email)
        {
            return new EmailParser(email);
        }
    }
}
