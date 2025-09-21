using System;

namespace WebApi.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}