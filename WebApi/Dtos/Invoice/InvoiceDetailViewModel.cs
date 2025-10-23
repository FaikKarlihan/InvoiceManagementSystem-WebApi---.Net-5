using System;

namespace WebApi.Dtos
{
    public class InvoiceDetailViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string PaymentInfo { get; set; }
        public decimal Amount { get; set; }
        public string Period { get; set; }
        public DateTime DueDate { get; set; }
        public string OverdueStatus   { get; set; }
        public string PaymentStatus { get; set; }
        public string ApartmentNumber { get; set; }
    }
}