using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Common;

namespace WebApi.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public InvoiceType Type { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string PaymentInfo { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NotPaid;
        public OverdueStatus OverdueStatus 
        { 
            get
            {
                if (PaymentStatus == PaymentStatus.Paid)
                    return OverdueStatus.NotOverdue;
        
                return DateTime.Now > DueDate ? OverdueStatus.Overdue : OverdueStatus.NotOverdue;
            }
        }
        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}