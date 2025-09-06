using MongoDB.Bson; //mongo db data types
using MongoDB.Bson.Serialization.Attributes; //mongo db attributes

namespace InventoryMLApp.Models
{
    public class InventoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = "";
        [BsonElement("storeId")]
        public string StoreId { get; set; } = "";
        [BsonElement("productId")]
        public string ProductId { get; set; } = "";
        [BsonElement("productName")]
        public string ProductName { get; set; } = "";
        [BsonElement("currentStock")]
        public int CurrentStock { get; set; }
        [BsonElement("minimumStock")]
        public int MinimumStock { get; set; }
        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        [BsonElement("pricePoint")]
        public decimal PricePoint { get; set; }
        [BsonElement("category")]
        public string Category { get; set; } = "";

        //generate unique id
        public void GenerateId()
        {
            Id = $"{StoreId}-{ProductId}-{DateTime.Now:yyyyMMddHHmmss}";
            
        }
    
    }
    
}