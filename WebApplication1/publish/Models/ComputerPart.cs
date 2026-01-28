using System;

namespace WebApplication1.Models
{
    public class ComputerPart
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public string PartName { get; set; }
        public string PartType { get; set; }
        public decimal PartCost { get; set; }
        public int PartStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string ThirdPartyOrder { get; set; }
        public int PurchasePlatform { get; set; }
    }
}
