using System;

namespace WebApplication1.Models
{
    public class ComputerMain
    {
        public int Id { get; set; }
        public string ComputerName { get; set; }
        public DateTime CreateTime { get; set; }
        public int SaleStatus { get; set; }
        public DateTime StatusTime { get; set; }
        public decimal CostAmount { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal PlatformCommission { get; set; }
        public decimal ProfitAmount { get; set; }
    }
    
    public class ComputerSummary
    {
        public int TotalCount { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal TotalPlatformCommission { get; set; }
        public decimal TotalProfitAmount { get; set; }
    }
}
