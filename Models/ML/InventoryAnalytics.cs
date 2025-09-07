using System.Runtime.Intrinsics.X86;

namespace InventoryMLApp.Models.ML
{
    public class InventoryAnalytics
    {

        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string StoreID { get; set; } = "";

        //historical metrics
        public float AverageDailyDemand { get; set; }
        public float DemandVariability { get; set; }
        public float SeasonalityStrength { get; set; }
        public float TrendStrength { get; set; }

        //performance metrics
        public float StockoutFrequency { get; set; }
        public float OverstockDays { get; set; }
        public float TurnoverRate { get; set; }

        //optimization suggestions
        public int OptimalMinimumStock { get; set; }
        public int OptimalMaximumStock { get; set; }
        public int OptimalReorderPoint { get; set; }
        public int OptimalReorderQuantity { get; set; }

        public DateTime LastAnalysed { get; set; }

    }
}