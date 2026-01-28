using System;
using System.Collections.Generic;
using WebApplication1.Bll;
using WebApplication1.Models;

namespace WebApplication1
{
    class TestComputerSystem
    {
        static void Main(string[] args)
        {
            Console.WriteLine("电脑装配库存系统测试");
            Console.WriteLine("====================");
            
            // 测试数据
            string testPartInfo = "主板\t七彩虹h610m-e\t190\n" +
                                 "cpu\ti5 12490F\t720\n" +
                                 "内存\t海盗船8*2 3200\t320\n" +
                                 "硬盘\t500G 三星固态.m2\t190\n" +
                                 "电源\t骨伽 sxc 500\t60\n" +
                                 "机箱\t黑色海景房\t70\n" +
                                 "散热器\t乔思伯 cr 1400evo 黑色四铜管\t45\n" +
                                 "显卡\t铭鑫显卡3060ti\t1230";
            
            // 创建测试对象
            var inputModel = new ComputerInputModel
            {
                ComputerName = "测试电脑" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                PartInfo = testPartInfo
            };
            
            try
            {
                // 测试保存电脑信息
                var computerBll = new ComputerBll();
                bool result = computerBll.SaveComputer(inputModel);
                
                if (result)
                {
                    Console.WriteLine("✓ 测试成功：电脑信息保存成功");
                    
                    // 测试获取电脑列表
                    List<ComputerMain> computers = computerBll.GetAllComputers();
                    Console.WriteLine($"✓ 测试成功：获取电脑列表，共 {computers.Count} 台电脑");
                    
                    if (computers.Count > 0)
                    {
                        // 测试获取最近添加的电脑信息
                        var latestComputer = computers[0];
                        Console.WriteLine($"✓ 测试成功：获取最近电脑信息，ID: {latestComputer.Id}, 名称: {latestComputer.ComputerName}");
                        
                        // 测试获取电脑配件
                        List<ComputerPart> parts = computerBll.GetComputerParts(latestComputer.Id);
                        Console.WriteLine($"✓ 测试成功：获取电脑配件，共 {parts.Count} 个配件");
                        
                        foreach (var part in parts)
                        {
                            Console.WriteLine($"  - {part.PartType}: {part.PartName} (成本: {part.PartCost})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("✗ 测试失败：电脑信息保存失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 测试异常：{ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            
            Console.WriteLine("\n测试完成，按任意键退出...");
            Console.ReadKey();
        }
    }
}