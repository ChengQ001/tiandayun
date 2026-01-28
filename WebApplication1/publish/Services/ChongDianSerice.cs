using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Tool;

namespace WebApplication1.Services
{
    public class ChongDianSerice
    {

        public async Task<String> selectOrderNum()
        {
            string response = "";
            try
            {
                // 设置请求头
                var headers = new WebHeaderCollection();
                headers.Add("X-Ca-Signature", "A784464BD4DBE0777505A4DC15D778EF");
                headers.Add("appVersion", "8.0.1.2");
                headers.Add("did", "");
                headers.Add("content-type", "application/x-www-form-urlencoded");
                headers.Add("userId", "f81fe01a-63fb-46d5-a0a3-ac7e4b40eee0");
                headers.Add("x-uid", "ozGb50HMpTtsTqe78NLIyCGH5iLU");
                headers.Add("channel-id", "100");
                headers.Add("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHBpcmVUaW1lIjoxNzY0OTAzNDY4NTU5LCJpc3MiOiJzdGFyQ2hhcmdlQXBwIiwiYXBwa2V5IjoiYjlFNTJCQTBpVjlpNDJkMTdjIiwiZXhwIjoxNzY0OTAzNDY4LCJpYXQiOjE3NjM2OTM4NjgsInVzZXJJZCI6ImY4MWZlMDFhLTYzZmItNDZkNS1hMGEzLWFjN2U0YjQwZWVlMCJ9.iWR8k6cWwVC8MqSpxNUZIPEQzZlA_7D4CCdzYon8sss");
                headers.Add("sid", "1763693821380");
                headers.Add("X-Ca-Timestamp", "1763711278664");
                headers.Add("positCity", "441900");
                headers.Add("Referer", "https://servicewechat.com/wxb8e2ba3a621b447d/294/page-frame.html");

                // 使用静态方法发送请求
                var httpHelper = new HttpHelper();
                httpHelper.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 18_6_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.65(0x1800412c) NetType/WIFI Language/zh_CN";

                string url = "https://gateway.sccncdn.com/apph5/xcxApiV2/wechat/charge/chargeCheck?nonce=ea464697-f112-489a-b8c9-adcf627e0e9d&stubId=10470599&timestamp=1763711278664";

                  response = await httpHelper.GetAsync(url, headers);
               
                Console.WriteLine("请求成功！");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 请求失败: {ex.Message}");
            }
            return response;
        }

      public    string MakeRequestSync()
        {
            try
            {
                string url = "https://gateway.sccncdn.com/apph5/xcxApiV2/wechat/charge/chargeCheck?nonce=ea464697-f112-489a-b8c9-adcf627e0e9d&stubId=10470599&timestamp=1763711278664";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 30000;

                // 设置请求头
                request.Headers["X-Ca-Signature"] = "A784464BD4DBE0777505A4DC15D778EF";
                request.Headers["appVersion"] = "8.0.1.2";
                request.Headers["did"] = "";
                request.Headers["userId"] = "f81fe01a-63fb-46d5-a0a3-ac7e4b40eee0";
                request.Headers["x-uid"] = "ozGb50HMpTtsTqe78NLIyCGH5iLU";
                request.Headers["channel-id"] = "100";
                request.Headers["sid"] = "1763693821380";
                request.Headers["X-Ca-Timestamp"] = "1763711278664";
                request.Headers["positCity"] = "441900";

                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = "https://servicewechat.com/wxb8e2ba3a621b447d/294/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 18_6_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.65(0x1800412c) NetType/WIFI Language/zh_CN";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Headers["Authorization"] = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHBpcmVUaW1lIjoxNzY0OTAzNDY4NTU5LCJpc3MiOiJzdGFyQ2hhcmdlQXBwIiwiYXBwa2V5IjoiYjlFNTJCQTBpVjlpNDJkMTdjIiwiZXhwIjoxNzY0OTAzNDY4LCJpYXQiOjE3NjM2OTM4NjgsInVzZXJJZCI6ImY4MWZlMDFhLTYzZmItNDZkNS1hMGEzLWFjN2U0YjQwZWVlMCJ9.iWR8k6cWwVC8MqSpxNUZIPEQzZlA_7D4CCdzYon8sss";

                Console.WriteLine("开始发送GET请求...");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"响应状态: {response.StatusCode}");

                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseText = reader.ReadToEnd();
                        return responseText;
                    }
                }
            }
            catch
            {
                
            }
            return "";

        }


    }



}

 