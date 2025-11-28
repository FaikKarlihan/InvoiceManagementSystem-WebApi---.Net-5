using System;

namespace WebApi.Dtos
{
    public class InvoiceCreateDto
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string PaymentInfo { get; set; }
        public string ApartmentNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime DueDate { get; set; }
    }
}
       