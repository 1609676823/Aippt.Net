using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Aippt.Net
{
   /// <summary>
   /// 
   /// </summary>
    public class AuthWebHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthWebHelper() { }
        #region web请求
        /// <summary>
        /// 超时
        /// </summary>
        public int RequestTimeoutMilliseconds { get; set; } = 300 * 1000;
        /// <summary>
        /// json选项设置
        /// </summary>
        public virtual JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions()
        {

            //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default,
            WriteIndented = true,
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            MaxDepth = int.MaxValue,
        };

        private bool useUnsafeRelaxedJsonEscaping = false;
        /// <summary>
        /// 使用对编码内容不太严格的内置 JavaScript 编码器实例
        /// </summary>
        public bool UseUnsafeRelaxedJsonEscaping
        {
            get { return useUnsafeRelaxedJsonEscaping; }
            set
            {
                if (value)
                {
                    JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true,
                        // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNameCaseInsensitive = true,
                        MaxDepth = int.MaxValue,
                    };
                }
                else
                {
                    JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        // Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.Always,
                        PropertyNameCaseInsensitive = true,
                        MaxDepth = int.MaxValue,
                    };

                }

                useUnsafeRelaxedJsonEscaping = value;
            }
        }

        /// <summary>
        /// 真实请求的body的记录，只是记录，不携带这个请求
        /// </summary>
        public string RealRequestBody { get; set; } = string.Empty;

        /// <summary>
        /// 真实请求的URL的记录，只是记录，不携带这个请求
        /// </summary>
        public string RealRequestUrl { get; set; } = string.Empty;
        /// <summary>
        /// 真实请求的FormData的记录,只是记录，不携带这个请求
        /// </summary>
        public MultipartFormDataContent? RealRequesFormData { get; set; } = new MultipartFormDataContent();

        /// <summary>
        /// 
        /// </summary>
        public HttpClient? httpClient;

        /// <summary>
        /// 
        /// </summary>

        public HttpResponseMessage? httpResponseMessage;

        /// <summary>
        /// 
        /// </summary>
        public Encoding EncodingDefault { get; set; } = Encoding.UTF8;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="queryParameters"></param>
        /// <param name="requestheaders"></param>
        /// <param name="multipartFormData"></param>
        /// <param name="postData"></param>
       
        /// <returns></returns>
        public virtual async Task<Byte[]> SendRequestAsByteAsync(HttpMethod method, string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, object? postData = null)
        {
            RealRequestBody = string.Empty;

            using (httpClient = new HttpClient())
            {

                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey);

                // Set the timeout for the request
                httpClient.Timeout = TimeSpan.FromMilliseconds(RequestTimeoutMilliseconds);

                Uri? RequestUri;
                if (queryParameters != null && queryParameters.Count > 0)
                {
                    RequestUri = new Uri(url + "?" + CreateQueryString(queryParameters)); // 设置请求的 URI  
                }
                else
                {
                    RequestUri = new Uri(url); // 设置请求的 URI  
                }

                RealRequestUrl = RequestUri.ToString();
                // Create the request message
                var request = new HttpRequestMessage(method, RequestUri);

               


                /*自定义请求头*/
                if (requestheaders != null && requestheaders.Count > 0)
                {
                    foreach (var item in requestheaders)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                // Set the request content if postData is provided
                //if (!string.IsNullOrEmpty(postData))


                if (postData != null)
                {
                    Type type = postData.GetType();
                    string json = string.Empty;
                    if (type != null && "String".Equals(type.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        json = postData.ToString()!;
                    }
                    else
                    {
                        json = System.Text.Json.JsonSerializer.Serialize(postData, JsonSerializerOptions);
                    }


                    RealRequestBody = json;
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                }

                if (multipartFormData != null)
                {
                    RealRequesFormData = multipartFormData;
                    request.Content = multipartFormData;
                }
                //try
                //{

                // Send the request and get the response
                // using (var response = await httpClient.SendAsync(request, streamResponse ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead))
                using (var response = await httpClient.SendAsync(request))
                {
                    // response.EnsureSuccessStatusCode();
                    // Get the full response content
                    byte[] responseByte = await response.Content.ReadAsByteArrayAsync();
                    httpResponseMessage = response;
                    return responseByte;

                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="queryParameters"></param>
        /// <param name="requestheaders"></param>
        /// <param name="multipartFormData"></param>
        /// <param name="postData"></param>
        
        /// <returns></returns>
        public virtual async Task<string> SendRequestAsync(HttpMethod method, string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, object? postData = null)
        {

            string responsecontent = string.Empty;
            try
            {
                Byte[] bytes = await SendRequestAsByteAsync(method, url, queryParameters, requestheaders, multipartFormData, postData);
                responsecontent = EncodingDefault.GetString(bytes);
                if (UseUnsafeRelaxedJsonEscaping)
                {
                    responsecontent = CommonHelper.DecodeJson(responsecontent);
                }
            }
            catch (Exception ex)
            {
                responsecontent = ex.Message;
                // throw;
            }
            return responsecontent;
        }


        /// <summary>
        /// FormData请求方式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryParameters"></param>
        /// <param name="requestheaders"></param>
        /// <param name="multipartFormData"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public virtual async Task<string> SendRequestByFormDataAsync(string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, object? postData = null)
        {
            // method = method ?? HttpMethod.Post;
            return await SendRequestAsync(HttpMethod.Post, url, queryParameters, requestheaders, multipartFormData);
        }
        /// <summary>
        /// 创建 Query 参数的字符串表示形式
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        // 创建 Query 参数的字符串表示形式
        public string CreateQueryString(Dictionary<string, string> parameters)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&");
                }
                queryString.Append(parameter.Key).Append("=").Append(WebUtility.UrlEncode(parameter.Value));
            }
            return queryString.ToString();
        }
        #endregion
    }
}
