using Microsoft.ML.Data;

namespace InventoryMLApp.Models.ML
{
    public class DemandPrediction
    {
        // Input features for training
        [LoadColumn(0)]
        public float DayOfYear { get; set; }
        
        [LoadColumn(1)]
        public float IsWeekend { get; set; }
        
        [LoadColumn(2)]
        public float IsHoliday { get; set; }
        
        [LoadColumn(3)]
        public float SeasonalIndex { get; set; }
        
        [LoadColumn(4)]
        public float MovingAverage7Days { get; set; }
        
        [LoadColumn(5)]
        public float MovingAverage30Days { get; set; }
        
        [LoadColumn(6)]
        public float PricePoint { get; set; }
        
        [LoadColumn(7)]
        public float CurrentStock { get; set; }
        
        // Target column - this is what we're predicting
        [LoadColumn(8)]
        [ColumnName("Label")]
        public float DemandQuantity { get; set; }
        
        // Prediction output (not used during training)
        [ColumnName("Score")]
        public float PredictedDemand { get; set; }
    }

    // Separate class for prediction results
    public class DemandPredictionResult
    {
        [ColumnName("Score")]
        public float PredictedDemand { get; set; }
    }

    // Historical demand data for training
    public class HistoricalDemandData
    {
        public DateTime Date { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string StoreId { get; set; } = string.Empty;
        public float DemandQuantity { get; set; }
        public float PricePoint { get; set; }
        public float CurrentStock { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    // Risk level enumeration
    public enum ForecastRisk
    {
        Low,
        Medium, 
        High,
        Critical
    }

    // Main forecast result class
    public class ForecastResult
    {
        public string ProductName { get; set; } = string.Empty;
        public string StoreId { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public float PredictedDemand { get; set; }
        public int DaysUntilStockout { get; set; }
        public ForecastRisk RiskLevel { get; set; }
        public int RecommendedReorderQuantity { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; } = new();
    }

    // Daily forecast details
    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public float PredictedDemand { get; set; }
        public int ProjectedStock { get; set; }
    }
}