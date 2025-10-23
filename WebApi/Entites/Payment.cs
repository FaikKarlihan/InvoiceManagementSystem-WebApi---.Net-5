using System;

namespace WebApi.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

    }
}