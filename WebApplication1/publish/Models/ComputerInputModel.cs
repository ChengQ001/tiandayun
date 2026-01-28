using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ComputerInputModel
    {
        [Required(ErrorMessage = "电脑名称不能为空")]
        public string ComputerName { get; set; }
        
        [Required(ErrorMessage = "配件信息不能为空")]
        public string PartInfo { get; set; }
    }
    
    /// <summary>
    /// 电脑配件输入模型
    /// </summary>
    public class ComputerPartInputModel
    {
        /// <summary>
        /// 配件类型
        /// </summary>
        [Required(ErrorMessage = "配件类型不能为空")]
        public string PartType { get; set; }
        
        /// <summary>
        /// 配件名称
        /// </summary>
        [Required(ErrorMessage = "配件名称不能为空")]
        public string PartName { get; set; }
        
        /// <summary>
        /// 配件成本
        /// </summary>
        [Required(ErrorMessage = "配件成本不能为空")]
        [Range(0.01, double.MaxValue, ErrorMessage = "配件成本必须大于0")]
        public decimal PartCost { get; set; }
        
        /// <summary>
        /// 配件状态
        /// </summary>
        public int PartStatus { get; set; }
        
        /// <summary>
        /// 第三方订单
        /// </summary>
        public string ThirdPartyOrder { get; set; }
        
        /// <summary>
        /// 购买平台
        /// </summary>
        public int PurchasePlatform { get; set; }
    }
}
