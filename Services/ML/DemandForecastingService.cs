using Microsoft.ML;
using Microsoft.ML.Data;
using InventoryMLApp.Models;
using InventoryMLApp.Models.ML;  // ← Import the ML models
using InventoryMLApp.Services;

namespace InventoryMLApp.Services.ML
{
    public class DemandForecastingService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;
        private readonly string _modelPath;
        private readonly InventoryService _inventoryService;

        public DemandForecastingService(InventoryService inventoryService)
        {
            _mlContext = new MLContext(seed: 0);
            _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ml-model.zip");
            _inventoryService = inventoryService;
            
            // Ensure wwwroot directory exists
            var wwwrootPath = Path.GetDirectoryName(_modelPath);
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath!);
            }
        }

        public Task<bool> LoadModelAsync() // ✅ Remove async since no await
{
    try
    {
        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading model: {ex.Message}");
        return Task.FromResult(false);
    }
}

public Task<bool> TrainModelAsync() // ✅ Remove async since no await
{
    try
    {
        Console.WriteLine("Starting model training...");
        
        // Generate synthetic training data
        var trainingData = GenerateSyntheticTrainingData();
        
        // Convert to IDataView
        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);
        
        // Create training pipeline
        var pipeline = _mlContext.Transforms.Concatenate("Features", 
                nameof(DemandPrediction.DayOfYear),
                nameof(DemandPrediction.IsWeekend), 
                nameof(DemandPrediction.IsHoliday),
                nameof(DemandPrediction.SeasonalIndex),
                nameof(DemandPrediction.MovingAverage7Days),
                nameof(DemandPrediction.MovingAverage30Days),
                nameof(DemandPrediction.PricePoint),
                nameof(DemandPrediction.CurrentStock))
            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

        Console.WriteLine("Training the model with regression algorithm...");
        
        // Train the model
        _model = pipeline.Fit(dataView);
        
        // Save the model
        _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
        
        Console.WriteLine($"Model trained and saved to: {_modelPath}");
        return Task.FromResult(true);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during model training: {ex.Message}");
        return Task.FromResult(false);
    }
}

        public async Task<List<ForecastResult>> GetAllForecastsAsync()
        {
            try
            {
                if (_model == null && !await LoadModelAsync())
                {
                    if (!await TrainModelAsync())
                    {
                        throw new InvalidOperationException("Could not train or load model");
                    }
                }

                var items = await _inventoryService.GetAllItemsAsync();
                var forecasts = new List<ForecastResult>();

                var predictionEngine = _mlContext.Model.CreatePredictionEngine<DemandPrediction, DemandPredictionResult>(_model!);

                foreach (var item in items)
                {
                    var prediction = PredictDemandForItem(item, predictionEngine);
                    forecasts.Add(prediction);
                }

                return forecasts.OrderByDescending(f => f.RiskLevel).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating forecasts: {ex.Message}");
                throw;
            }
        }

        private ForecastResult PredictDemandForItem(InventoryItem item, PredictionEngine<DemandPrediction, DemandPredictionResult> predictionEngine)
{
    var today = DateTime.Now;
    
    // Create input for prediction with explicit type conversions
    var input = new DemandPrediction
    {
        DayOfYear = (float)today.DayOfYear,
        IsWeekend = today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday ? 1.0f : 0.0f,
        IsHoliday = 0.0f,
        SeasonalIndex = CalculateSeasonalIndex(today),
        MovingAverage7Days = (float)item.CurrentStock * 0.1f,
        MovingAverage30Days = (float)item.CurrentStock * 0.05f,
        PricePoint = (float)item.PricePoint, // ✅ Convert decimal to float for ML
        CurrentStock = (float)item.CurrentStock
    };

    // Rest of your method remains the same...
    var prediction = predictionEngine.Predict(input);
    var predictedDemand = Math.Max(0, prediction.PredictedDemand);

    var daysUntilStockout = predictedDemand > 0 ? (int)(item.CurrentStock / predictedDemand) : 999;

    var riskLevel = daysUntilStockout switch
    {
        <= 1 => ForecastRisk.Critical,
        <= 7 => ForecastRisk.High,
        <= 30 => ForecastRisk.Medium,
        _ => ForecastRisk.Low
    };

    var recommendedReorder = riskLevel switch
    {
        ForecastRisk.Critical => (int)(predictedDemand * 14),
        ForecastRisk.High => (int)(predictedDemand * 7),
        ForecastRisk.Medium => (int)(predictedDemand * 3),
        _ => 0
    };

    return new ForecastResult
    {
        ProductName = item.ProductName, // ✅ Use ProductName for display
        StoreId = item.StoreId,
        CurrentStock = item.CurrentStock,
        PredictedDemand = predictedDemand,
        DaysUntilStockout = daysUntilStockout,
        RiskLevel = riskLevel,
        RecommendedReorderQuantity = recommendedReorder,
        DailyForecasts = GenerateDailyForecasts(item, predictedDemand)
    };
}

        private List<DailyForecast> GenerateDailyForecasts(InventoryItem item, float avgDemand)
        {
            var forecasts = new List<DailyForecast>();
            var currentStock = (float)item.CurrentStock;
            var random = new Random();

            for (int i = 0; i < 7; i++)
            {
                // Add some variation to daily demand
                var dailyDemand = Math.Max(0, avgDemand + (random.NextSingle() - 0.5f) * avgDemand * 0.3f);
                currentStock = Math.Max(0, currentStock - dailyDemand);

                forecasts.Add(new DailyForecast
                {
                    Date = DateTime.Now.AddDays(i),
                    PredictedDemand = dailyDemand,
                    ProjectedStock = (int)currentStock
                });
            }

            return forecasts;
        }

        private float CalculateSeasonalIndex(DateTime date)
        {
            // Simple seasonal calculation - you can make this more sophisticated
            var monthIndex = date.Month / 12.0f;
            return (float)(0.8 + 0.4 * Math.Sin(2 * Math.PI * monthIndex));
        }

        private List<DemandPrediction> GenerateSyntheticTrainingData()
        {
            var trainingData = new List<DemandPrediction>();
            var random = new Random(42); // Fixed seed for reproducible results

            // Generate 1000 synthetic data points
            for (int i = 0; i < 1000; i++)
            {
                var date = DateTime.Now.AddDays(-random.Next(365));
                var baseStock = random.Next(10, 1000);
                var pricePoint = (float)(random.NextDouble() * 100 + 10);
                
                // Create realistic demand based on factors
                var seasonalFactor = CalculateSeasonalIndex(date);
                var weekendFactor = (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) ? 0.7f : 1.0f;
                var priceFactor = Math.Max(0.1f, 2.0f - (pricePoint / 50.0f));
                
                var demandQuantity = Math.Max(1, (float)baseStock * 0.05f * seasonalFactor * weekendFactor * priceFactor + 
                                            (float)random.NextGaussian() * 5);

                trainingData.Add(new DemandPrediction
                {
                    DayOfYear = (float)date.DayOfYear,
                    IsWeekend = weekendFactor < 1.0f ? 1.0f : 0.0f,
                    IsHoliday = random.NextDouble() < 0.05 ? 1.0f : 0.0f,
                    SeasonalIndex = seasonalFactor,
                    MovingAverage7Days = demandQuantity * 0.9f + (float)random.NextGaussian() * 2,
                    MovingAverage30Days = demandQuantity * 0.8f + (float)random.NextGaussian() * 3,
                    PricePoint = pricePoint,
                    CurrentStock = (float)baseStock,
                    DemandQuantity = demandQuantity
                });
            }

            return trainingData;
        }
    }

    // ✅ KEEP ONLY THE EXTENSION METHOD HERE
    public static class RandomExtensions
    {
        public static double NextGaussian(this Random random, double mean = 0.0, double stdDev = 1.0)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }
    }

    
}