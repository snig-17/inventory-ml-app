using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryMLApp.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("storeId")]
        public string StoreId { get; set; } = string.Empty;

        [BsonElement("productId")]
        public string ProductId { get; set; } = string.Empty;

        [BsonElement("productName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("currentStock")]
        public int CurrentStock { get; set; }

        [BsonElement("minimumStock")]
        public int MinimumStock { get; set; }

        // ✅ FIX: Change from float to decimal to handle precision properly
        [BsonElement("pricePoint")]
        [BsonRepresentation(BsonType.Decimal128)] // ← Handle decimal precision
        public decimal PricePoint { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public void GenerateId()
        {
            if (string.IsNullOrEmpty(Id))
        {
                Id = ObjectId.GenerateNewId().ToString();
            }
        }
    }
}