using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Tool
{

    /// <summary>
    /// HTTP请求工具类
    /// </summary>
    public class HttpHelper
    {
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";

        /// <summary>
        /// 超时时间（毫秒）
        /// </summary>
        public int Timeout { get; set; } = 10000;

        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; set; } = DefaultUserAgent;

        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding Encoding { get; set; } = DefaultEncoding;

        /// <summary>
        /// Cookie容器
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        public HttpHelper()
        {
            CookieContainer = new CookieContainer();
        }

        /// <summary>
        /// 发送GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串</returns>
        public string Get(string url, WebHeaderCollection headers = null)
        {
            return SendRequest(url, "GET", null, headers);
        }

        /// <summary>
        /// 发送GET请求（异步）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串任务</returns>
        public async Task<string> GetAsync(string url, WebHeaderCollection headers = null)
        {
            return await SendRequestAsync(url, "GET", null, headers);
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">提交数据</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串</returns>
        public string Post(string url, string data = null, string contentType = "application/x-www-form-urlencoded", WebHeaderCollection headers = null)
        {
            return SendRequest(url, "POST", data, headers, contentType);
        }

        /// <summary>
        /// 发送POST请求（异步）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">提交数据</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串任务</returns>
        public async Task<string> PostAsync(string url, string data = null, string contentType = "application/x-www-form-urlencoded", WebHeaderCollection headers = null)
        {
            return await SendRequestAsync(url, "POST", data, headers, contentType);
        }

        /// <summary>
        /// 发送POST JSON请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonData">JSON数据</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串</returns>
        public string PostJson(string url, string jsonData, WebHeaderCollection headers = null)
        {
            if (headers == null)
                headers = new WebHeaderCollection();

            headers.Add("Content-Type", "application/json");
            return Post(url, jsonData, "application/json", headers);
        }

        /// <summary>
        /// 发送POST JSON请求（异步）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="jsonData">JSON数据</param>
        /// <param name="headers">请求头</param>
        /// <returns>响应字符串任务</returns>
        public async Task<string> PostJsonAsync(string url, string jsonData, WebHeaderCollection headers = null)
        {
            if (headers == null)
                headers = new WebHeaderCollection();

            headers.Add("Content-Type", "application/json");
            return await PostAsync(url, jsonData, "application/json", headers);
        }

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        private string SendRequest(string url, string method, string data, WebHeaderCollection headers = null, string contentType = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = CreateRequest(url, method, headers, contentType);

                if (!string.IsNullOrEmpty(data) && (method == "POST" || method == "PUT"))
                {
                    byte[] postData = Encoding.GetBytes(data);
                    request.ContentLength = postData.Length;

                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(postData, 0, postData.Length);
                    }
                }

                response = (HttpWebResponse)request.GetResponse();
                return GetResponseString(response);
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    return GetResponseString(response);
                }
                throw new HttpRequestException($"HTTP请求失败: {ex.Message}", ex);
            }
            finally
            {
                response?.Close();
            }
        }

        /// <summary>
        /// 发送HTTP请求（异步）
        /// </summary>
        private async Task<string> SendRequestAsync(string url, string method, string data, WebHeaderCollection headers = null, string contentType = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = CreateRequest(url, method, headers, contentType);

                if (!string.IsNullOrEmpty(data) && (method == "POST" || method == "PUT"))
                {
                    byte[] postData = Encoding.GetBytes(data);
                    request.ContentLength = postData.Length;

                    using (Stream stream = await request.GetRequestStreamAsync())
                    {
                        await stream.WriteAsync(postData, 0, postData.Length);
                    }
                }

                response = (HttpWebResponse)await request.GetResponseAsync();
                return await GetResponseStringAsync(response);
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    return await GetResponseStringAsync(response);
                }
                throw new HttpRequestException($"HTTP请求失败: {ex.Message}", ex);
            }
            finally
            {
                response?.Close();
            }
        }

        /// <summary>
        /// 创建HTTP请求
        /// </summary>
        private HttpWebRequest CreateRequest(string url, string method, WebHeaderCollection headers, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = Timeout;
            request.UserAgent = UserAgent;
            request.CookieContainer = CookieContainer;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            return request;
        }

        /// <summary>
        /// 获取响应字符串
        /// </summary>
        private string GetResponseString(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null) return string.Empty;

                using (StreamReader reader = new StreamReader(stream, Encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 获取响应字符串（异步）
        /// </summary>
        private async Task<string> GetResponseStringAsync(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null) return string.Empty;

                using (StreamReader reader = new StreamReader(stream, Encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }

    /// <summary>
    /// HTTP请求异常
    /// </summary>
    public class HttpRequestException : Exception
    {
        public HttpRequestException(string message) : base(message) { }
        public HttpRequestException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 简化使用的静态方法
    /// </summary>
    public static class HttpSimple
    {
        private static readonly HttpHelper _httpHelper = new HttpHelper();

        public static string Get(string url)
        {
            return _httpHelper.Get(url);
        }

        public static async Task<string> GetAsync(string url)
        {
            return await _httpHelper.GetAsync(url);
        }

        public static string Post(string url, string data = null, string contentType = "application/x-www-form-urlencoded")
        {
            return _httpHelper.Post(url, data, contentType);
        }

        public static async Task<string> PostAsync(string url, string data = null, string contentType = "application/x-www-form-urlencoded")
        {
            return await _httpHelper.PostAsync(url, data, contentType);
        }

        public static string PostJson(string url, string jsonData)
        {
            return _httpHelper.PostJson(url, jsonData);
        }

        public static async Task<string> PostJsonAsync(string url, string jsonData)
        {
            return await _httpHelper.PostJsonAsync(url, jsonData);
        }
    }
}
