using Dapper;
using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1.Tool;

namespace WebApplication1.Services
{
    public class ComputerPartService
    {
        /// <summary>
        /// 批量添加电脑配件
        /// </summary>
        /// <param name="parts">配件列表</param>
        /// <returns>影响行数</returns>
        public int AddComputerParts(List<ComputerPart> parts)
        {
            if (parts == null || parts.Count == 0)
                return 0;

            string sql = "INSERT INTO ComputerPart (ComputerId, PartName, PartType, PartCost, PartStatus, CreateTime, ThirdPartyOrder, PurchasePlatform) " +
                         "VALUES (@ComputerId, @PartName, @PartType, @PartCost, @PartStatus, @CreateTime, @ThirdPartyOrder, @PurchasePlatform)";
            
            return DapperHelper.Execute(sql, parts);
        }

        /// <summary>
        /// 根据电脑ID获取配件列表
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>配件列表</returns>
        public List<ComputerPart> GetPartsByComputerId(int computerId)
        {
            string sql = "SELECT * FROM ComputerPart WHERE ComputerId = @ComputerId";
            var parameters = new Dictionary<string, object>();
            parameters.Add("ComputerId", computerId);
            return DapperHelper.Query<ComputerPart>(sql, parameters).AsList();
        }

        /// <summary>
        /// 更新配件状态
        /// </summary>
        /// <param name="id">配件ID</param>
        /// <param name="status">配件状态</param>
        /// <returns>影响行数</returns>
        public int UpdatePartStatus(int id, int status)
        {
            string sql = "UPDATE ComputerPart SET PartStatus = @Status WHERE Id = @Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            parameters.Add("Status", status);
            return DapperHelper.Execute(sql, parameters);
        }
        
        /// <summary>
        /// 根据电脑ID删除所有配件
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>影响行数</returns>
        public int DeletePartsByComputerId(int computerId)
        {
            string sql = "DELETE FROM ComputerPart WHERE ComputerId = @ComputerId";
            var parameters = new Dictionary<string, object>();
            parameters.Add("ComputerId", computerId);
            return DapperHelper.Execute(sql, parameters);
        }
        
        /// <summary>
        /// 更新电脑配件信息
        /// </summary>
        /// <param name="part">配件信息</param>
        /// <returns>影响行数</returns>
        public int UpdateComputerPart(ComputerPart part)
        {
            string sql = "UPDATE ComputerPart SET " +
                        "PartType = @PartType, " +
                        "PartName = @PartName, " +
                        "PartCost = @PartCost, " +
                        "PartStatus = @PartStatus, " +
                        "ThirdPartyOrder = @ThirdPartyOrder, " +
                        "PurchasePlatform = @PurchasePlatform " +
                        "WHERE Id = @Id";
            return DapperHelper.Execute(sql, part);
        }
        
        /// <summary>
        /// 根据配件ID获取配件信息
        /// </summary>
        /// <param name="partId">配件ID</param>
        /// <returns>配件信息</returns>
        public ComputerPart GetComputerPartById(int partId)
        {
            string sql = "SELECT * FROM ComputerPart WHERE Id = @Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", partId);
            return DapperHelper.QueryFirstOrDefault<ComputerPart>(sql, parameters);
        }
    }
}
