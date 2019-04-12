using API.Services.Models;

namespace API.Services
{
    public interface IBookingService
    {
        DetailBooking GetDetailBooking(string email);
    }
}
