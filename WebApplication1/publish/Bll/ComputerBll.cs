using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Tool;

namespace WebApplication1.Bll
{
    public class ComputerBll
    {
        private readonly ComputerService _computerService;
        private readonly ComputerPartService _computerPartService;

        public ComputerBll()
        {
            _computerService = new ComputerService();
            _computerPartService = new ComputerPartService();
        }

        /// <summary>
        /// 保存电脑信息
        /// </summary>
        /// <param name="inputModel">前端输入模型</param>
        /// <returns>保存结果</returns>
        public bool SaveComputer(ComputerInputModel inputModel)
        {
            try
            {
                // 解析配件信息
                var parts = ParsePartInfo(inputModel.PartInfo);
                if (parts == null || parts.Count == 0)
                    return false;

                // 计算总成本
                decimal totalCost = parts.Sum(p => p.PartCost);

                // 创建电脑主表记录
                var computer = new ComputerMain
                {
                    ComputerName = inputModel.ComputerName,
                    CreateTime = DateTime.Now,
                    SaleStatus = 1, // 1待安装
                    StatusTime = DateTime.Now,
                    CostAmount = totalCost,
                    SaleAmount = 0,
                    PlatformCommission = 0,
                    ProfitAmount = 0
                };

                // 添加电脑主表
                int computerId = _computerService.AddComputer(computer);
                if (computerId <= 0)
                    return false;

                // 设置配件的电脑ID
                parts.ForEach(p =>
                {
                    p.ComputerId = computerId;
                    p.CreateTime = DateTime.Now;
                });

                // 批量添加配件
                int affectedRows = _computerPartService.AddComputerParts(parts);
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 解析配件信息
        /// </summary>
        /// <param name="partInfo">配件信息文本</param>
        /// <returns>配件列表</returns>
        private List<ComputerPart> ParsePartInfo(string partInfo)
        {
            if (string.IsNullOrWhiteSpace(partInfo))
                return null;

            var parts = new List<ComputerPart>();
            var lines = partInfo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // 跳过空行
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // 按制表符分割，格式：配件类型	配件名称	成本
                var partsInfo = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (partsInfo.Length < 3)
                    continue;

                // 解析成本
                if (!decimal.TryParse(partsInfo[2], out decimal cost))
                    continue;

                // 创建配件对象
                var part = new ComputerPart
                {
                    PartType = partsInfo[0].Trim(),
                    PartName = partsInfo[1].Trim(),
                    PartCost = cost,
                    PartStatus = 1, // 1正常
                    ThirdPartyOrder = string.Empty,
                    PurchasePlatform = 1 // 1咸鱼
                };

                parts.Add(part);
            }

            return parts;
        }

        /// <summary>
        /// 获取所有电脑信息
        /// </summary>
        /// <returns>电脑列表</returns>
        public List<ComputerMain> GetAllComputers()
        {
            return _computerService.GetAllComputers();
        }
        
        /// <summary>
        /// 分页获取电脑信息
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="searchKeyword">搜索关键词</param>
        /// <param name="saleStatus">销售状态</param>
        /// <returns>电脑列表</returns>
        public List<ComputerMain> GetComputersByPage(int pageIndex, int pageSize, out int totalCount, string searchKeyword = null, int? saleStatus = null)
        {
            return _computerService.GetComputersByPage(pageIndex, pageSize, out totalCount, searchKeyword, saleStatus);
        }
        
        /// <summary>
        /// 根据ID获取电脑信息
        /// </summary>
        /// <param name="id">电脑ID</param>
        /// <returns>电脑信息</returns>
        public ComputerMain GetComputerById(int id)
        {
            return _computerService.GetComputerById(id);
        }

        /// <summary>
        /// 获取电脑配件列表
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>配件列表</returns>
        public List<ComputerPart> GetComputerParts(int computerId)
        {
            return _computerPartService.GetPartsByComputerId(computerId);
        }
        
        /// <summary>
        /// 更新电脑销售状态
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <param name="saleStatus">销售状态</param>
        /// <param name="saleAmount">售出金额</param>
        /// <param name="platformCommission">平台提成</param>
        /// <returns>更新结果</returns>
        public bool UpdateSaleStatus(int computerId, int saleStatus, decimal saleAmount, decimal platformCommission)
        {
            try
            {
                // 获取电脑信息
                var computer = _computerService.GetComputerById(computerId);
                if (computer == null)
                    return false;
                
                // 计算利润
                decimal profitAmount = saleAmount - computer.CostAmount - platformCommission;
                
                // 更新电脑信息
                computer.SaleStatus = saleStatus;
                computer.StatusTime = DateTime.Now;
                computer.SaleAmount = saleAmount;
                computer.PlatformCommission = platformCommission;
                computer.ProfitAmount = profitAmount;
                
                // 调用服务层更新
                int affectedRows = _computerService.UpdateComputer(computer);
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 删除电脑
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>删除结果</returns>
        public bool DeleteComputer(int computerId)
        {
            try
            {
                // 先删除配件数据
                int partsAffected = _computerPartService.DeletePartsByComputerId(computerId);
                
                // 再删除主表数据
                int mainAffected = _computerService.DeleteComputer(computerId);
                
                // 返回删除结果
                return mainAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 通过表单方式保存电脑信息
        /// </summary>
        /// <param name="computerName">电脑名称</param>
        /// <param name="parts">配件列表</param>
        /// <returns>保存结果</returns>
        public bool SaveComputerForm(string computerName, IEnumerable<ComputerPartInputModel> parts)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(computerName) || parts == null || !parts.Any())
                    return false;

                // 计算总成本
                decimal totalCost = parts.Sum(p => p.PartCost);

                // 创建电脑主表记录
                var computer = new ComputerMain
                {
                    ComputerName = computerName,
                    CreateTime = DateTime.Now,
                    SaleStatus = 1, // 1待安装
                    StatusTime = DateTime.Now,
                    CostAmount = totalCost,
                    SaleAmount = 0,
                    PlatformCommission = 0,
                    ProfitAmount = 0
                };

                // 添加电脑主表
                int computerId = _computerService.AddComputer(computer);
                if (computerId <= 0)
                    return false;

                // 转换为配件实体
                var computerParts = parts.Select(p => new ComputerPart
                {
                    ComputerId = computerId,
                    PartType = p.PartType,
                    PartName = p.PartName,
                    PartCost = p.PartCost,
                    PartStatus = p.PartStatus > 0 ? p.PartStatus : 1, // 默认正常
                    ThirdPartyOrder = p.ThirdPartyOrder ?? string.Empty,
                    PurchasePlatform = p.PurchasePlatform > 0 ? p.PurchasePlatform : 1, // 默认咸鱼
                    CreateTime = DateTime.Now
                }).ToList();

                // 批量添加配件
                int affectedRows = _computerPartService.AddComputerParts(computerParts);
                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 更新配件信息并重新计算电脑成本和利润
        /// </summary>
        /// <param name="part">配件信息</param>
        /// <returns>更新结果</returns>
        public bool UpdateComputerPartAndRecalculate(ComputerPart part)
        {
            try
            {
                if (part == null || part.Id <= 0)
                    return false;
                
                // 更新配件信息
                int partAffectedRows = _computerPartService.UpdateComputerPart(part);
                if (partAffectedRows <= 0)
                    return false;
                
                // 重新计算电脑成本和利润
                int computerAffectedRows = _computerService.UpdateComputerCostAndProfit(part.ComputerId);
                
                return computerAffectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 根据配件ID获取配件信息
        /// </summary>
        /// <param name="partId">配件ID</param>
        /// <returns>配件信息</returns>
        public ComputerPart GetComputerPartById(int partId)
        {
            return _computerPartService.GetComputerPartById(partId);
        }
        
        /// <summary>
        /// 获取已售电脑汇总数据
        /// </summary>
        /// <returns>汇总数据</returns>
        public ComputerSummary GetSoldComputersSummary()
        {
            return _computerService.GetSoldComputersSummary();
        }
    }
}
