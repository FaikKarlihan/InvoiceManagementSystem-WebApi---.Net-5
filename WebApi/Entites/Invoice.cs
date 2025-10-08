using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string PaymentInfo { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}