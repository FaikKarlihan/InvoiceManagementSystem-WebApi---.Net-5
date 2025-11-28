using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Models
{
    public class User
    {
        [JsonIgnore]
        [BsonId] // MongoDB’deki _id alanını temsil eder
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserId { get; set; } // Ana API’deki user’ın ID’si ile eşleşecek
        // Banka bilgileri (simülasyon)
        public string CardNumber { get; set; }
        public string Password { get; set; }
        
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }
    }
}