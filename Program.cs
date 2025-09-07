using InventoryMLApp.Components; // ← Add this import
using InventoryMLApp.Services;
using InventoryMLApp.Services.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MongoDB connection
var connectionString = "mongodb+srv://inventoryuser:GWgK7qO2Ty7m1ySD@inventorycluster.vjbrkwd.mongodb.net/?retryWrites=true&w=majority";

// Register services
builder.Services.AddSingleton<InventoryService>(provider =>
    new InventoryService(connectionString));

builder.Services.AddSingleton<DemandForecastingService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();