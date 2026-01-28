using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Tool;

namespace WebApplication1.Services
{
    public class ComputerService
    {
        /// <summary>
        /// 添加电脑信息
        /// </summary>
        /// <param name="computer">电脑信息</param>
        /// <returns>电脑ID</returns>
        public int AddComputer(ComputerMain computer)
        {
            string sql = "INSERT INTO ComputerMain (ComputerName, CreateTime, SaleStatus, StatusTime, CostAmount, SaleAmount, PlatformCommission, ProfitAmount) " +
                         "VALUES (@ComputerName, @CreateTime, @SaleStatus, @StatusTime, @CostAmount, @SaleAmount, @PlatformCommission, @ProfitAmount);" +
                         "SELECT LAST_INSERT_ID();";
            return DapperHelper.ExecuteScalar<int>(sql, computer);
        }

        /// <summary>
        /// 获取所有电脑信息
        /// </summary>
        /// <returns>电脑列表</returns>
        public List<ComputerMain> GetAllComputers()
        {
            string sql = "SELECT * FROM ComputerMain ORDER BY CreateTime DESC";
            return DapperHelper.Query<ComputerMain>(sql).ToList();
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
            // 计算偏移量
            int offset = (pageIndex - 1) * pageSize;
            
            // 构建查询条件
            var conditions = new List<string>();
            var param = new Dictionary<string, object>();
            
            // 搜索关键词处理
            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                conditions.Add("ComputerName LIKE @SearchKeyword");
                param["SearchKeyword"] = $"%{searchKeyword}%";
            }
            
            // 销售状态处理
            if (saleStatus.HasValue && saleStatus.Value > 0)
            {
                conditions.Add("SaleStatus = @SaleStatus");
                param["SaleStatus"] = saleStatus.Value;
            }
            
            // 构建WHERE子句
            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
            
            // 查询总记录数
            string countSql = $"SELECT COUNT(*) FROM ComputerMain {whereClause}";
            var countResult = DapperHelper.Query<int>(countSql, param);
            totalCount = countResult.FirstOrDefault();
            
            // 查询分页数据
            string querySql = $"SELECT * FROM ComputerMain {whereClause} ORDER BY CreateTime DESC LIMIT {pageSize} OFFSET {offset}";
            return DapperHelper.Query<ComputerMain>(querySql, param).ToList();
        }
        
        /// <summary>
        /// 根据ID获取电脑信息
        /// </summary>
        /// <param name="id">电脑ID</param>
        /// <returns>电脑信息</returns>
        public ComputerMain GetComputerById(int id)
        {
            string sql = "SELECT * FROM ComputerMain WHERE Id = @Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return DapperHelper.QueryFirstOrDefault<ComputerMain>(sql, parameters);
        }

        /// <summary>
        /// 更新电脑销售状态
        /// </summary>
        /// <param name="id">电脑ID</param>
        /// <param name="status">销售状态</param>
        /// <returns>影响行数</returns>
        public int UpdateComputerStatus(int id, int status)
        {
            string sql = "UPDATE ComputerMain SET SaleStatus = @Status, StatusTime = NOW() WHERE Id = @Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            parameters.Add("Status", status);
            return DapperHelper.Execute(sql, parameters);
        }
        
        /// <summary>
        /// 更新电脑信息
        /// </summary>
        /// <param name="computer">电脑信息</param>
        /// <returns>影响行数</returns>
        public int UpdateComputer(ComputerMain computer)
        {
            string sql = "UPDATE ComputerMain SET SaleStatus = @SaleStatus, StatusTime = @StatusTime, " +
                         "SaleAmount = @SaleAmount, PlatformCommission = @PlatformCommission, ProfitAmount = @ProfitAmount " +
                         "WHERE Id = @Id";
            return DapperHelper.Execute(sql, computer);
        }
        
        /// <summary>
        /// 删除电脑
        /// </summary>
        /// <param name="id">电脑ID</param>
        /// <returns>影响行数</returns>
        public int DeleteComputer(int id)
        {
            string sql = "DELETE FROM ComputerMain WHERE Id = @Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return DapperHelper.Execute(sql, parameters);
        }
        
        /// <summary>
        /// 更新电脑总成本和利润
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>影响行数</returns>
        public int UpdateComputerCostAndProfit(int computerId)
        {
            // 查询电脑所有配件的总成本
            string costSql = "SELECT SUM(PartCost) FROM ComputerPart WHERE ComputerId = @ComputerId";
            var costParams = new Dictionary<string, object>();
            costParams.Add("ComputerId", computerId);
            decimal totalCost = DapperHelper.ExecuteScalar<decimal>(costSql, costParams);
            
            // 获取电脑当前信息
            string computerSql = "SELECT * FROM ComputerMain WHERE Id = @Id";
            var computerParams = new Dictionary<string, object>();
            computerParams.Add("Id", computerId);
            var computer = DapperHelper.QueryFirstOrDefault<ComputerMain>(computerSql, computerParams);
            if (computer == null)
                return 0;
            
            // 重新计算利润
            decimal profitAmount = computer.SaleAmount - totalCost - computer.PlatformCommission;
            
            // 更新电脑主表
            string updateSql = "UPDATE ComputerMain SET " +
                             "CostAmount = @CostAmount, " +
                             "ProfitAmount = @ProfitAmount " +
                             "WHERE Id = @Id";
            
            var updateParams = new Dictionary<string, object>();
            updateParams.Add("CostAmount", totalCost);
            updateParams.Add("ProfitAmount", profitAmount);
            updateParams.Add("Id", computerId);
            
            return DapperHelper.Execute(updateSql, updateParams);
        }
        
        /// <summary>
        /// 获取已售电脑汇总数据
        /// </summary>
        /// <returns>汇总数据</returns>
        public ComputerSummary GetSoldComputersSummary()
        {
            // 先查询所有电脑的总成本
            string totalCostSql = "SELECT COALESCE(SUM(CostAmount), 0) as TotalCostAmount FROM ComputerMain";
            var totalCostResult = DapperHelper.Query<decimal>(totalCostSql);
            decimal totalCost = totalCostResult.FirstOrDefault();
            
            // 再查询已售电脑的其他汇总数据
            string soldSql = "SELECT " +
                            "COUNT(*) as TotalCount, " +
                            "COALESCE(SUM(SaleAmount), 0) as TotalSaleAmount, " +
                            "COALESCE(SUM(PlatformCommission), 0) as TotalPlatformCommission, " +
                            "COALESCE(SUM(ProfitAmount), 0) as TotalProfitAmount " +
                            "FROM ComputerMain " +
                            "WHERE SaleStatus = 4";
            
            var soldSummary = DapperHelper.QueryFirstOrDefault<dynamic>(soldSql);
            
            // 构建返回结果
            var summary = new ComputerSummary {
                TotalCount = (int)(soldSummary?.TotalCount ?? 0),
                TotalSaleAmount = soldSummary?.TotalSaleAmount ?? 0m,
                TotalCostAmount = totalCost,
                TotalPlatformCommission = soldSummary?.TotalPlatformCommission ?? 0m,
                TotalProfitAmount = soldSummary?.TotalProfitAmount ?? 0m
            };
            
            return summary;
        }
    }
}
