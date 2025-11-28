using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PaymentApi.Models
{
    public class PaymentRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PaymentToken { get; set; } = Guid.NewGuid().ToString();
        public int UserId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
