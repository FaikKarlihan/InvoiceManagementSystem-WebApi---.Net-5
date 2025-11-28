using System;

namespace WebApi.Dtos
{
    public class TransactionDto
    {
        public int UserId { get; set; }       // ana API user id
        public int InvoiceId { get; set; }    // ödenen fatura id
        public decimal Amount { get; set; }   // tutar
        public DateTime TransactionDate { get; set; }
    }
}