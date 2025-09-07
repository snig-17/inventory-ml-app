# 🤖 InventoryMLApp - AI-Powered Smart Inventory Management System

A modern, cloud-native inventory management system built with **Blazor**, **MongoDB Atlas**, and **Machine Learning** capabilities for intelligent stock tracking and AI-powered predictive restocking.

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download)
[![MongoDB](https://img.shields.io/badge/MongoDB-Atlas-green.svg)](https://www.mongodb.com/atlas)
[![Blazor](https://img.shields.io/badge/Blazor-Server-purple.svg)](https://blazor.net)
[![ML.NET](https://img.shields.io/badge/ML.NET-4.0-orange.svg)](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)]()

## 🚀 Overview

InventoryMLApp is a **next-generation AI-powered inventory management system** designed for modern businesses that need intelligent stock tracking, real-time updates, and machine learning-driven demand forecasting. Built using cutting-edge Microsoft technologies and cloud services with integrated artificial intelligence capabilities.

### 🎯 Key Features

- ✅ **Real-Time Inventory Tracking** - Live updates across all connected users
- ✅ **Cloud-Native Architecture** - Powered by MongoDB Atlas for global scalability  
- ✅ **AI-Powered Demand Forecasting** - ML.NET predictive analytics for stock optimization
- ✅ **Intelligent Risk Assessment** - Four-tier risk system (Critical/High/Medium/Low)
- ✅ **Smart Reorder Recommendations** - AI-suggested optimal restock quantities
- ✅ **Modern Web Interface** - Responsive Blazor Server UI with Bootstrap styling
- ✅ **Multi-Store Support** - Manage inventory across multiple locations
- ✅ **ML Dashboard** - Comprehensive AI insights and forecasting visualization
- ✅ **Data Persistence** - Reliable cloud database storage with automatic backups
- ✅ **Professional Dashboard** - Clean, intuitive interface for business users

### 🤖 AI & Machine Learning Features

- 🧠 **Demand Forecasting Engine** - Predicts future stock needs using ML.NET regression models
- 📊 **Seasonal Pattern Recognition** - Identifies sales trends, weekend effects, and holiday patterns
- ⚡ **Automated Risk Assessment** - Real-time stockout risk calculation and alerts
- 📈 **Predictive Analytics Dashboard** - Visual insights with daily forecasts and projections
- 🎯 **Smart Reordering System** - AI-driven optimal stock level recommendations
- 📉 **Historical Data Analysis** - Synthetic data generation for model training
- 🔮 **Multi-Factor Predictions** - Considers seasonality, pricing, stock levels, and temporal patterns

## 🛠️ Technology Stack

### Frontend
- **Blazor Server** (.NET 9.0) - Interactive web UI with C#
- **Bootstrap 5** - Modern, responsive styling
- **Chart Components** - Interactive ML visualization dashboards
- **Real-time UI Updates** - Live ML predictions and risk assessments

### Backend
- **ASP.NET Core 9.0** - High-performance web framework
- **C# 12** - Modern, type-safe programming language
- **Dependency Injection** - Clean architecture patterns
- **ML.NET 4.0** - Microsoft's machine learning framework

### Machine Learning Stack
- **ML.NET 4.0.2** - Core machine learning framework
- **ML.NET AutoML 0.22.2** - Automated machine learning capabilities
- **ML.NET TimeSeries 4.0.2** - Time series forecasting models
- **SDCA Regression** - Stochastic Dual Coordinate Ascent algorithm
- **Feature Engineering** - Multi-dimensional data processing

### Database
- **MongoDB Atlas** - Cloud-native NoSQL database
- **MongoDB Driver** (2.28.0) - Official .NET MongoDB client
- **Time-Series Optimization** - Efficient inventory and ML data tracking
- **Flexible Schema** - Handles both inventory data and ML training datasets

### Cloud & DevOps
- **MongoDB Atlas** - Fully managed database service
- **Cross-Platform** - Runs on Windows, macOS, Linux
- **ML Model Persistence** - Automated model saving and loading
- **Scalable Architecture** - Ready for enterprise deployment

## 🏃‍♂️ Quick Start

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB Atlas Account](https://cloud.mongodb.com) (Free tier available)
- Modern web browser (Chrome, Firefox, Safari, Edge)

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/InventoryMLApp.git
   cd InventoryMLApp
   ```
1. **Install Dependencies**
    ```bash
   dotnet restore
   ```
1. **Configure Database Connection**
    ```bash
    // Update Program.cs with your MongoDB Atlas connection string
    var connectionString = "mongodb+srv://username:password@cluster.mongodb.net/";
   ```
1. **Build and Run**
    ```bash
    dotnet build
    dotnet run
   ```
1. **Access Application**
    ```bash
    Open: https://localhost:5014
    Navigate to: /simple-inventory
   ```
## 🎮 Usage Examples
### Add New Product
```bash
var newProduct = new InventoryItem
{
    StoreId = "store1",
    ProductId = "iphone15",
    ProductName = "iPhone 15",
    CurrentStock = 50,
    MinimumStock = 10,
    PricePoint = 999.99m,
    Category = "Electronics"
};

await inventoryService.CreateAsync(newProduct);
```
### Update Stock Levels
```bash
// Add 25 units to existing stock
await inventoryService.UpdateStockAsync(productId, currentStock + 25);
```
### Query Low Stock Items
```bash
var lowStockItems = await inventoryService.GetLowStockItemsAsync();
```
## 🔧 Configuration

### Database Settings
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://user:password@cluster.mongodb.net/InventoryDatabase"
  },
  "DatabaseSettings": {
    "DatabaseName": "InventoryDatabase",
    "CollectionName": "InventoryItems"
  },
  "MLSettings": {
    "ModelPath": "wwwroot/ml-model.zip",
    "TrainingDataSize": 1000,
    "MaxIterations": 100
  }
}
```

## 🧪 Development Roadmap

### Phase 1: Core Features ✅ *Complete*

- [x] Basic CRUD operations
- [x] MongoDB Atlas integration
- [x] Responsive web interface
- [x] Multi-store support
- [x] Data persistence

### Phase 2: Machine Learning ✅ *Complete*

- [x] ML.NET integration
- [x] Demand forecasting models
- [x] Risk assessment system
- [x] AI Dashboard with visualizations
- [x] Automated model training
- [x] Smart reorder recommendations

### Phase 3: Advanced ML Features 🔄 *In Progress*

- [ ] Azure ML integration
- [ ] Advanced seasonal pattern recognition
- [ ] Multi-product correlation analysis
- [ ] Custom model algorithms
- [ ] A/B testing for ML models

### Phase 4: Real-Time & Enterprise 📈 *Planned*

- [ ] Real-time ML predictions with SignalR
- [ ] Advanced analytics dashboard
- [ ] Report generation with ML insights
- [ ] REST API for ML services
- [ ] Mobile app with AI features

### ML Features Used

1. **DayOfYear** - Temporal patterns (1-365)
2. **IsWeekend** - Weekend effect (0/1)
3. **IsHoliday** - Holiday impact (0/1)
4. **SeasonalIndex** - Monthly seasonality (0.4-1.2)
5. **MovingAverage7Days** - Short-term trend
6. **MovingAverage30Days** - Long-term trend
7. **PricePoint** - Product pricing impact
8. **CurrentStock** - Inventory level context

### Risk Assessment Algorithm

- **Critical**: ≤ 1 day until stockout
- **High**: 2-7 days until stockout
- **Medium**: 8-30 days until stockout
- **Low**: 30+ days of stock remaining

## 🤝 Contributing

Contributions are welcome! Here's how to get started:

1. **Fork the Repository**
2. **Create Feature Branch** (`git checkout -b feature/MLEnhancement`)
3. **Commit Changes** (`git commit -m 'Add advanced ML feature'`)
4. **Push to Branch** (`git push origin feature/MLEnhancement`)
5. **Open Pull Request**

### Development Guidelines

- Follow C# coding conventions
- Add unit tests for ML components
- Update ML model documentation
- Ensure cross-platform ML compatibility
- Test ML predictions with sample data

### ML Development Notes

- Models are saved to `wwwroot/ml-model.zip`
- Synthetic training data is generated automatically
- Model retraining can be triggered via UI
- All ML operations are logged to console

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🏆 Acknowledgments

- **Microsoft** - For .NET, Blazor, and ML.NET frameworks
- **MongoDB** - For the powerful Atlas cloud database platform
- **Bootstrap Team** - For the responsive UI framework
- **ML.NET Team** - For the comprehensive machine learning toolkit