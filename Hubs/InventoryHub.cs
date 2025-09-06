using Microsoft.AspNetCore.SignalR;

namespace InventoryMLApp.Hubs
{
    public class InventoryHub : Hub
    {
        //when a client connects
        public async Task JoinInventoryGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "InventoryUpdates");
            await Clients.Caller.SendAsync("StatusUpdate", "Connected to Inventory Updates");
        }
        //when a client disconnects
        public async Task LeaveInventoryGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "InventoryUpdates");
        }
        //notify clients of stock update
        public async Task NotifyStockUpdate(string productName, int newStock, string storeId)
        {
            var message = $"{productName} stock updated to {newStock} units in {storeId}.";
            await Clients.Group("InventoryUpdates").SendAsync("StockUpdate", productName, newStock, storeId, message);
        }
        //notify about new product addition
        public async Task NotifyNewProduct(string productName, string storeId)
        {
            var message = $"New product {productName} added to {storeId}.";
            await Clients.Group("InventoryUpdates").SendAsync("NewProduct", productName, storeId, message);
        }
        //notify about low stock alert
        public async Task NotifyLowStock(string productName, int currentStock, int minimumStock)
        {
            var message = $"Alert: {productName} stock is low = ({currentStock}/{minimumStock})";
            await Clients.Group("InventoryUpdates").SendAsync("LowStockAlert", productName, currentStock, minimumStock, message);
        }
    }
}
