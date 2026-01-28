using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Bll;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ComputerController : Controller
    {
        private readonly ComputerBll _computerBll;

        public ComputerController()
        {
            _computerBll = new ComputerBll();
        }

        /// <summary>
        /// 电脑列表页面
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 电脑录入页面（文本解析方式）
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Create()
        {
            return View();
        }
        
        /// <summary>
        /// 电脑录入页面（表单方式）
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult CreateForm()
        {
            return View();
        }
        
        /// <summary>
        /// 保存电脑信息（文本解析方式）
        /// </summary>
        /// <param name="inputModel">前端输入模型</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Save(ComputerInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                bool result = _computerBll.SaveComputer(inputModel);
                if (result)
                {
                    return Json(new { success = true, message = "保存成功" });
                }
                else
                {
                    return Json(new { success = false, message = "保存失败" });
                }
            }
            else
            {
                return Json(new { success = false, message = "数据验证失败" });
            }
        }
        
        /// <summary>
        /// 保存电脑信息（表单方式）
        /// </summary>
        /// <param name="computerName">电脑名称</param>
        /// <param name="parts">配件列表</param>
        /// <returns>保存结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveForm(string computerName, IEnumerable<ComputerPartInputModel> parts)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(computerName))
                {
                    return Json(new { success = false, message = "电脑名称不能为空" });
                }
                
                if (parts == null || !parts.Any())
                {
                    return Json(new { success = false, message = "至少需要添加一个配件" });
                }
                
                // 调用BLL保存电脑信息
                bool result = _computerBll.SaveComputerForm(computerName, parts);
                return Json(new { success = result, message = result ? "保存成功" : "保存失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "保存失败：" + ex.Message });
            }
        }

        /// <summary>
        /// 获取电脑列表JSON数据
        /// </summary>
        /// <returns>电脑列表JSON</returns>
        [AllowAnonymous]
        public ActionResult GetComputerList()
        {
            var computers = _computerBll.GetAllComputers();
            return Json(computers, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 分页获取电脑列表JSON数据
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="searchKeyword">搜索关键词</param>
        /// <param name="saleStatus">销售状态</param>
        /// <returns>分页电脑列表JSON</returns>
        [AllowAnonymous]
        public ActionResult GetComputerListByPage(int pageIndex = 1, int pageSize = 20, string searchKeyword = null, int? saleStatus = null)
        {
            int totalCount;
            var computers = _computerBll.GetComputersByPage(pageIndex, pageSize, out totalCount, searchKeyword, saleStatus);
            
            return Json(new {
                items = computers,
                totalCount = totalCount,
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 根据电脑ID获取配件列表
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>配件列表JSON</returns>
        [AllowAnonymous]
        public ActionResult GetComputerParts(int computerId)
        {
            var parts = _computerBll.GetComputerParts(computerId);
            return Json(parts, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 更新电脑销售状态
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <param name="saleStatus">销售状态</param>
        /// <param name="saleAmount">售出金额</param>
        /// <param name="platformCommission">平台提成</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateSaleStatus(int computerId, int saleStatus, decimal saleAmount, decimal platformCommission)
        {
            try
            {
                bool result = _computerBll.UpdateSaleStatus(computerId, saleStatus, saleAmount, platformCommission);
                return Json(new { success = result, message = result ? "更新成功" : "更新失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "更新失败：" + ex.Message });
            }
        }
        
        /// <summary>
        /// 删除电脑
        /// </summary>
        /// <param name="computerId">电脑ID</param>
        /// <returns>删除结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteComputer(int computerId)
        {
            try
            {
                bool result = _computerBll.DeleteComputer(computerId);
                return Json(new { success = result, message = result ? "删除成功" : "删除失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "删除失败：" + ex.Message });
            }
        }
        
        /// <summary>
        /// 更新电脑配件信息
        /// </summary>
        /// <param name="part">配件信息</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateComputerPart(ComputerPart part)
        {
            try
            {
                bool result = _computerBll.UpdateComputerPartAndRecalculate(part);
                return Json(new { success = result, message = result ? "更新成功" : "更新失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "更新失败：" + ex.Message });
            }
        }
        
        /// <summary>
        /// 获取已售电脑汇总数据
        /// </summary>
        /// <returns>汇总数据JSON</returns>
        [AllowAnonymous]
        public ActionResult GetSoldComputersSummary()
        {
            var summary = _computerBll.GetSoldComputersSummary();
            return Json(summary, JsonRequestBehavior.AllowGet);
        }
    }
}
