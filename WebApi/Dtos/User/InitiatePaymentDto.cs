namespace WebApi.Dtos
{
    public class InitiatePaymentDto
    {
        public int UserId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}