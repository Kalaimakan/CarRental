namespace CarRental.ViewModels
{
    public class PaymentViewModel
    {
        public Guid BookingId { get; set; }
        public string CarModel { get; set; }
        public string Dates { get; set; }
        public decimal Total { get; set; }

        public string Method { get; set; }

        // Credit/Debit
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }

        // PayPal
        public string PayPalEmail { get; set; }
    }
}
