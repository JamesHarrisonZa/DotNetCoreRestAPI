namespace API.Services.Models
{
	public class EmailBooking
	{
		public string CostCentre { get; set; }
		public decimal Total { get; set; }
		public string PaymentMethod { get; set; }
		public string Vendor { get; set; }
		public string Description { get; set; }
		public string Date { get; set; }

		public EmailBooking(string costCentre, decimal total, string paymentMethod, string vendor, string description, string date)
		{
			CostCentre = costCentre;
			Total = total;
			PaymentMethod = paymentMethod;
			Vendor = vendor;
			Description = description;
			Date = date;
		}
	}
}