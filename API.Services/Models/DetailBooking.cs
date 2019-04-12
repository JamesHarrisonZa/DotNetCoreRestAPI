namespace API.Services.Models
{
	public class DetailBooking
	{
		public string CostCentre { get; set; }
		public string PaymentMethod { get; set; }
		public string Vendor { get; set; }
		public string Description { get; set; }
		public string Date { get; set; }
		public decimal Total { get; set; }
		public decimal Gst { get; set; }
		public decimal TotalExcludingGst { get; set; }

		//Decorator pattern. Using composition over inheritence
		public DetailBooking(EmailBooking emailBooking, decimal gst, decimal totalExcludingGst)
		{
			CostCentre = emailBooking.CostCentre;
			PaymentMethod = emailBooking.PaymentMethod;
			Vendor = emailBooking.Vendor;
			Description = emailBooking.Description;
			Date = emailBooking.Date;
			Total = emailBooking.Total;
			Gst = gst;
			TotalExcludingGst = totalExcludingGst;
		}
	}
}
