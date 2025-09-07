using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.ML.Data;

namespace InventoryMLApp.Models.ML
{
    public class DemandPrediction
{
    [LoadColumn(0)]
    public string StoreID { get; set; } = "";
    [LoadColumn(1)]
    public string ProductId { get; set; } = "";
    [LoadColumn(2)]
    public float DayOfYear { get; set; }
    [LoadColumn(3)]
    public float DayOfWeek { get; set; }
    [LoadColumn(4)]
    public float IsWeekend { get; set; }
    [LoadColumn(5)]
    public float IsHoliday { get; set; }
    [LoadColumn(6)]
    public float PricePoint { get; set; }
    [LoadColumn(7)]
    public float MovingAverage7Days { get; set; }
    [LoadColumn(8)]
    public float MovingAverage30Days { get; set; }
    [LoadColumn(9)]
    public float SeasonalIndex { get; set; }
    [LoadColumn(10)]
    public float TrendFactor { get; set; }
    [LoadColumn(11)]
    public float WeatherIndex { get; set; }
    [LoadColumn(12)]
    [ColumnName("Label")]
    public float DemandQuantity { get; set; }
}

public class DemandPredictionResult
{
    [ColumnName("Score")]
    public float PredictedDemand { get; set; }
}
}