using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using InventoryMLApp.Models;

namespace InventoryMLApp.Services
{
    public class InventoryService
    {
        private readonly IMongoCollection<InventoryItem> _inventoryItems;
        private readonly IMongoDatabase _database;

        public InventoryService(string connectionString)
        {
            try
            {
                Console.WriteLine($"🔗 Connecting to MongoDB...");
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase("InventoryDatabase");
                _inventoryItems = _database.GetCollection<InventoryItem>("InventoryItems");
                Console.WriteLine("✅ MongoDB connection established");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ MongoDB connection failed: {ex.Message}");
                throw;
            }
        }

        // ✅ Database connection test
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                Console.WriteLine("🧪 Testing database connection...");
                var result = await _database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("✅ Database ping successful");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database ping failed: {ex.Message}");
                return false;
            }
        }

        // Get all inventory items
        public async Task<List<InventoryItem>> GetAllAsync()
        {
            try
            {
                Console.WriteLine("📋 Fetching all items from database...");
                var items = await _inventoryItems.Find(_ => true).ToListAsync();
                Console.WriteLine($"📦 Found {items.Count} items in database");
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching items: {ex.Message}");
                throw;
            }
        }

        // ✅ Alias method for ML service
        public async Task<List<InventoryItem>> GetAllItemsAsync()
        {
            return await GetAllAsync();
        }

        // Get items by store
        public async Task<List<InventoryItem>> GetByStoreAsync(string storeId)
        {
            try
            {
                Console.WriteLine($"🏪 Fetching items for store: {storeId}");
                var items = await _inventoryItems.Find(item => item.StoreId == storeId).ToListAsync();
                Console.WriteLine($"📦 Found {items.Count} items for store {storeId}");
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching items for store {storeId}: {ex.Message}");
                throw;
            }
        }

        // Add new item
        public async Task CreateAsync(InventoryItem item)
        {
            try
            {
                Console.WriteLine($"➕ Creating new item: {item.ProductId}");
                item.GenerateId();
                item.LastUpdated = DateTime.UtcNow;
                await _inventoryItems.InsertOneAsync(item);
                Console.WriteLine($"✅ Item created successfully: {item.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error creating item: {ex.Message}");
                throw;
            }
        }

        // ✅ Boolean return version
        public async Task<bool> CreateItemAsync(InventoryItem item)
        {
            try
            {
                await CreateAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ CreateItemAsync failed: {ex.Message}");
                return false;
            }
        }

        // Update existing item
        public async Task UpdateAsync(string id, InventoryItem item)
        {
            try
            {
                Console.WriteLine($"📝 Updating item: {id}");
                item.LastUpdated = DateTime.UtcNow;
                var result = await _inventoryItems.ReplaceOneAsync(i => i.Id == id, item);
                Console.WriteLine($"✅ Update result - Modified: {result.ModifiedCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating item {id}: {ex.Message}");
                throw;
            }
        }

        // Delete item
        public async Task DeleteAsync(string id)
        {
            try
            {
                Console.WriteLine($"🗑️ Deleting item: {id}");
                var result = await _inventoryItems.DeleteOneAsync(item => item.Id == id);
                Console.WriteLine($"✅ Delete result - Deleted: {result.DeletedCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting item {id}: {ex.Message}");
                throw;
            }
        }

        // Update stock level
        public async Task UpdateStockAsync(string id, int newStock)
        {
            try
            {
                Console.WriteLine($"📦 Updating stock for {id} to {newStock}");
                var update = Builders<InventoryItem>.Update
                    .Set(item => item.CurrentStock, newStock)
                    .Set(item => item.LastUpdated, DateTime.UtcNow);

                var result = await _inventoryItems.UpdateOneAsync(item => item.Id == id, update);
                Console.WriteLine($"✅ Stock update result - Modified: {result.ModifiedCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating stock for {id}: {ex.Message}");
                throw;
            }
        }

        // Get item by ID
        public async Task<InventoryItem?> GetByIdAsync(string id)
        {
            try
            {
                Console.WriteLine($"🔍 Fetching item by ID: {id}");
                var item = await _inventoryItems.Find(item => item.Id == id).FirstOrDefaultAsync();
                Console.WriteLine(item != null ? $"✅ Found item: {item.ProductId}" : "❌ Item not found");
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching item {id}: {ex.Message}");
                throw;
            }
        }

        // Get low stock items
        public async Task<List<InventoryItem>> GetLowStockItemsAsync()
        {
            try
            {
                Console.WriteLine("⚠️ Fetching low stock items...");
                var items = await _inventoryItems.Find(item => item.CurrentStock <= item.MinimumStock).ToListAsync();
                Console.WriteLine($"📦 Found {items.Count} low stock items");
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching low stock items: {ex.Message}");
                throw;
            }
        }

        // ✅ Debug method - Get raw document sample
        public async Task<string> GetRawDocumentSampleAsync()
        {
            try
            {
                Console.WriteLine("🔍 Fetching raw document sample...");
                var collection = _database.GetCollection<BsonDocument>("InventoryItems");
                var sample = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();
                
                if (sample != null)
                {
                    Console.WriteLine($"📄 Raw document: {sample.ToJson()}");
                    return sample.ToJson();
                }
                return "No documents found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching raw document: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        // ✅ Migration method - Migrate database field names
        public async Task<bool> MigrateFieldNamesAsync()
        {
            try
            {
                Console.WriteLine("🔄 Starting field migration...");
                
                var collection = _database.GetCollection<BsonDocument>("InventoryItems");
                
                // Find all documents with 'productName' field
                var filter = BsonSerializer.Deserialize<BsonDocument>("{ productName: { $exists: true } }");
                var documents = await collection.Find(filter).ToListAsync();
                
                Console.WriteLine($"📦 Found {documents.Count} documents to migrate");
                
                foreach (var doc in documents)
                {
                    if (doc.Contains("productName"))
                    {
                        // Copy productName to productId and remove productName
                        doc["productId"] = doc["productName"];
                        doc.Remove("productName");
                        
                        // Update the document
                        var idFilter = Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]);
                        await collection.ReplaceOneAsync(idFilter, doc);
                    }
                }
                
                Console.WriteLine("✅ Field migration completed");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Migration error: {ex.Message}");
                return false;
            }
        }
    }
}