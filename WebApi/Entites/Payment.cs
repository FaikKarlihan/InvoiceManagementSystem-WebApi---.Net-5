using System;

namespace WebApi.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        
        public string UserName { get; set; }
        public string UserMail { get; set; }
    }
}