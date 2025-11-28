using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PaymentApi.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }       // ana API user id
        public int InvoiceId { get; set; }    // ödenen fatura id
        public decimal Amount { get; set; }   // tutar
        public DateTime TransactionDate { get; set; }
    }
}