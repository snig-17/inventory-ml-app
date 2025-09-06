using MongoDB.Driver;
using InventoryMLApp.Models;

namespace InventoryMLApp.Services
{
    public class InventoryService
    {
        private readonly IMongoCollection<InventoryItem> _inventoryItems;

        public InventoryService(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("InventoryDatabase");
            _inventoryItems = database.GetCollection<InventoryItem>("InventoryItems");
        }

        // Get all inventory items
        public async Task<List<InventoryItem>> GetAllAsync()
        {
            return await _inventoryItems.Find(_ => true).ToListAsync();
        }

        // Get items by store
        public async Task<List<InventoryItem>> GetByStoreAsync(string storeId)
        {
            return await _inventoryItems.Find(item => item.StoreId == storeId).ToListAsync();
        }

        // Add new item
        public async Task CreateAsync(InventoryItem item)
        {
            item.GenerateId();
            item.LastUpdated = DateTime.UtcNow;
            await _inventoryItems.InsertOneAsync(item);
        }

        // Update existing item
        public async Task UpdateAsync(string id, InventoryItem item)
        {
            item.LastUpdated = DateTime.UtcNow;
            await _inventoryItems.ReplaceOneAsync(i => i.Id == id, item);
        }

        // Delete item
        public async Task DeleteAsync(string id)
        {
            await _inventoryItems.DeleteOneAsync(item => item.Id == id);
        }

        // Update stock level
        public async Task UpdateStockAsync(string id, int newStock)
        {
            var update = Builders<InventoryItem>.Update
                .Set(item => item.CurrentStock, newStock)
                .Set(item => item.LastUpdated, DateTime.UtcNow);
            
            await _inventoryItems.UpdateOneAsync(item => item.Id == id, update);
        }

        // Get item by ID
        public async Task<InventoryItem?> GetByIdAsync(string id)
        {
            return await _inventoryItems.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        // Get low stock items
        public async Task<List<InventoryItem>> GetLowStockItemsAsync()
        {
            return await _inventoryItems.Find(item => item.CurrentStock <= item.MinimumStock).ToListAsync();
        }
    }
}
