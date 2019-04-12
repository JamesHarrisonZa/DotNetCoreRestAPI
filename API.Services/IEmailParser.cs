using API.Services.Models;

namespace API.Services
{
    public interface IEmailParser
    {
        EmailBooking GetEmailBooking(string email);
    }
}
