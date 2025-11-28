namespace PaymentApi.Models.DTOs
{
    public class InitiatePaymentDto
    {
        public int UserId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
    
    public class ConfirmPaymentDto
    {
        public string PaymentToken { get; set; }  // Kullanıcıya gönderilen şifre
        public string CardNumber { get; set; }
        public string CardPassword { get; set; }
    }
}
