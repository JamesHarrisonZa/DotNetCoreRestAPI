using NUnit.Framework;
using API.Services.Models;
using API.Services.Tests.Helpers;

namespace API.Services.Tests
{
    [TestFixture]
    public class BookingServiceTests
    {
        [TestCase(@"
		Hi Yvaine, 
		Please create an expense claim for the below.  Relevant details are marked up as requested… 
		
		<expense><cost_centre>DEV002</cost_centre> <total>1024.01</total><payment_method>personal card</payment_method> </expense> 
		
		From: Ivan Castle  Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd <Antoine.Lloyd@example.com> Subject: test 
		
		Hi Antoine,   
		
		Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>.  We expect to arrive around 7.15pm.  Approximately 12 people but I’ll confirm exact numbers closer to the day. 
		
		Regards, Ivan"
        , "DEV002", "personal card", "Viaduct Steakhouse", "development team’s project end celebration dinner", "Tuesday 27 April 2017, please double check this date as it may not be valid", 1024.01, 153.6015, 870.4085)]
        public void GetDetailBooking(string email, string costCentre, string paymentMethod, string vendor, string description, string date, decimal total, decimal gst, decimal totalExcludingGst)
        {
            var sut = CreateSut();
            var actual = sut.GetDetailBooking(email);
            var expected = new DetailBooking(new EmailBooking(costCentre, total, paymentMethod, vendor, description, date), gst, totalExcludingGst);

            var structuralEqualityComparer = new StructuralEqualityComparer();
            Assert.That(actual, Is.EqualTo(expected).Using(structuralEqualityComparer));
        }

        private BookingService CreateSut()
        {
            return new BookingService(new EmailParser(), new Calculator());
        }
    }
}
