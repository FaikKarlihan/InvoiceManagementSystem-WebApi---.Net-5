using System.ComponentModel.DataAnnotations;
using System;

namespace WebApi.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(100)]
        public string PaymentInfo { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int HousingId { get; set; }
        public Housing Housing { get; set; }

    }
}