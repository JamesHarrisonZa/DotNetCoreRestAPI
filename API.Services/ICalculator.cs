
namespace API.Services
{
    public interface ICalculator
    {
        decimal GetGst(decimal totalIncludingGst);
        decimal GetTotalExcludingGst(decimal totalIncludingGst, decimal gst);
    }
}
