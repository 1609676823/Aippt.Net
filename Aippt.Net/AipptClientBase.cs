using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using Aippt.Net.Model;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Text.Json.Nodes;

namespace Aippt.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class AipptClientBase : IDisposable
    {
        /// <summary>
        /// 最后一个Client的实例
        /// </summary>
        public static AipptClientBase? Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AipptClientBase() { Instance = this; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="secretkey"></param>
        /// <param name="userid">用户唯一性标记</param>
        /// <param name="apiurl">地址格式为： https://co.aippt.cn/</param>
        /// <param name="channel">渠道标识， 存在则进行传递， 没有传空</param>
        public AipptClientBase(string apikey, string secretkey, string userid, string apiurl = "https://co.aippt.cn/", string channel = "")
        {
            this.ApiKey = apikey;
            this.SecretKey = secretkey;
            this.Userid = userid;
            this.Url = apiurl;
            this.Channel = channel;
            Instance = this;
        }

        /// <summary>
        /// API KEY
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// SECRET KEY
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 用户唯一性标记
        /// </summary>
        public string Userid { get; set; } = string.Empty;

        /// <summary>
        /// 渠道标识， 存在则进行传递， 没有传空
        /// </summary>
        public string Channel { get; set; } = string.Empty;

        /// <summary>
        /// 不传则为PC，2则为移动端
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /*************自定义函数************/
        #region 获取token鉴权
        /// <summary>
        /// TokenResponse
        /// </summary>
        public TokenResponse GetTokenResponse { get; set; } = new TokenResponse();
        /// <summary>
        /// 
        /// </summary>
        public TokenResponse GetToken()
        {
            #region 获取TokenResponse
            Model.TokenResponse tokenResponse = new Model.TokenResponse();

            long timestamp = CommonHelper.TimestampSeconds();
            string data = "GET@/api/grant/token/@" + timestamp;
            string signature = GenHmac(data, this.SecretKey);
            Dictionary<string, string> headerdic = new Dictionary<string, string>();
            headerdic.Add("x-api-key", this.ApiKey);
            headerdic.Add("x-timestamp", timestamp.ToString());
            headerdic.Add("x-signature", signature);

            //Console.WriteLine("x-api-key: " + ApiKey);
            //Console.WriteLine("x-timestamp: " + timestamp);
            //Console.WriteLine("x-signature: " + signature);

            Dictionary<string, string> querydic = new Dictionary<string, string>();
            querydic.Add("uid", this.Userid);
            querydic.Add("channel", this.Channel);

            if (!string.IsNullOrWhiteSpace(Type))
            {
                querydic.Add("type", this.Type);
            }

            string url = this.Url + $"api/grant/token" + "?" + CreateQueryString(querydic);
            Task<string> task = new AuthWebHelper().SendRequestAsync(method: HttpMethod.Get, url: url, requestheaders: headerdic);
            task.Wait();
            string resjson = task.Result;
            tokenResponse.RealJsonstring = resjson;

            GetTokenResponse = tokenResponse;

            #endregion

            if (string.IsNullOrWhiteSpace(tokenResponse.data!.token))
            {
                throw new Exception("token is null;token获取失败");
            }

            return tokenResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> GetApiAuthHeadersByToken()
        {
            Dictionary<string, string> dicAuthHeaders = new Dictionary<string, string>();
            TokenResponse tokenResponse = this.GetToken();
            dicAuthHeaders.Add("x-api-key", this.ApiKey);
            dicAuthHeaders.Add("x-channel", this.Channel);
            dicAuthHeaders.Add("x-token", tokenResponse.data!.token!);
            return dicAuthHeaders;
        }

        #endregion

        #region 获取code鉴权

        /// <summary>
        /// code鉴权的返回结果
        /// </summary>
        public CodeResponse GetCodeResponse { get; set; } = new CodeResponse();

        /// <summary>
        /// 获取Code
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CodeResponse GetCode()
        {
            #region 获取CodeResponse
            Model.CodeResponse codeResponse = new Model.CodeResponse();

            long timestamp = CommonHelper.TimestampSeconds();
            string data = "GET@/api/grant/code/@" + timestamp;
            string signature = GenHmac(data, this.SecretKey);
            Dictionary<string, string> headerdic = new Dictionary<string, string>();
            headerdic.Add("x-api-key", this.ApiKey);
            headerdic.Add("x-timestamp", timestamp.ToString());
            headerdic.Add("x-signature", signature);

            //Console.WriteLine("x-api-key: " + ApiKey);
            //Console.WriteLine("x-timestamp: " + timestamp);
            //Console.WriteLine("x-signature: " + signature);

            Dictionary<string, string> querydic = new Dictionary<string, string>();
            querydic.Add("uid", this.Userid);
            querydic.Add("channel", this.Channel);

            if (!string.IsNullOrWhiteSpace(Type))
            {
                querydic.Add("type", this.Type);
            }

            string url = this.Url + $"api/grant/code" + "?" + CreateQueryString(querydic);
            Task<string> task = new AuthWebHelper().SendRequestAsync(method: HttpMethod.Get, url: url, requestheaders: headerdic);
            task.Wait();
            string resjson = task.Result;
            codeResponse.RealJsonstring = resjson;

            GetCodeResponse = codeResponse;

            #endregion

            if (string.IsNullOrWhiteSpace(GetCodeResponse.data!.code))
            {
                throw new Exception("token is null;code获取失败");
            }

            return codeResponse;
        }
        /// <summary>
        /// 获取code鉴权的字典
        /// </summary>
        /// <returns></returns>

        public Dictionary<string, string> GetAuthDictionaryByCode()
        {
            Dictionary<string, string> dicAuthHeaders = new Dictionary<string, string>();
            CodeResponse codeResponse = this.GetCode();
            dicAuthHeaders.Add("appkey", this.ApiKey);
            dicAuthHeaders.Add("channel", this.Channel);
            dicAuthHeaders.Add("code", codeResponse.code.ToString());
            return dicAuthHeaders;
        }

        #endregion
        /// <summary>
        /// HmacSHA1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GenHmac(string data, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (HMAC hmac = new HMACSHA1(keyBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] rawHmac = hmac.ComputeHash(dataBytes);
                return Convert.ToBase64String(rawHmac);
            }
        }
        /*************************/

        #region URL
        private string url = "https://co.aippt.cn/";
        /// <summary>
        /// api地址
        /// </summary>
        public string Url
        {
            get { return url; }
            set
            {
                url = GetServerUrl(value);
            }
        }
        /// <summary>
        /// url地址补充/
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string GetServerUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            try
            {
                if (!url.EndsWith("/"))
                {
                    return url + "/";
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return url;
        }
        #endregion

        #region IObservable订阅
        /// <summary>
        /// 可观察序列 数据流返回的结果
        /// </summary>
        public IObservable<string> AllObservDataReceived => _allObservDataReceived.AsObservable();

        /// <summary>
        /// 
        /// </summary>
        private readonly Subject<string> _allObservDataReceived = new Subject<string>();

        /// <summary>
        /// 返回 App 输出的流式块
        /// </summary>
        public IObservable<MeaasgeResponse> ObservMessageReceived => _observMessageReceived.AsObservable();
        private Subject<MeaasgeResponse> _observMessageReceived = new Subject<MeaasgeResponse>();
        #endregion

        #region Event订阅
        /// <summary>
        /// 事件处理器
        /// </summary>
        public event EventHandler<string> AllEventDataReceived = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        private void OnAllEventDataReceived(string response)
        {
            this.AllEventDataReceived?.Invoke(this, response);
        }
        /// <summary>
        ///返回 App 输出的流式块 事件处理器
        /// </summary>
        public event EventHandler<MeaasgeResponse> EventMessageReceived = delegate { };
        private void OnEventMessageReceived(MeaasgeResponse meaasgeResponseBase)
        {
            this.EventMessageReceived?.Invoke(this, meaasgeResponseBase);
        }

        #endregion

        /// <summary>
        /// SSE返回数据处理
        /// </summary>
        /// <param name="ChunknResponse"></param>
        /// <param name="ApiType"></param>
        /// <param name="Eventtype"></param>
        public virtual void ProcessServerSentEventsData(string ChunknResponse, string ApiType = "",string Eventtype="")
        {

            if (string.IsNullOrWhiteSpace(ChunknResponse)) { return; }
            if (ChunknResponse.IndexOf("data", StringComparison.OrdinalIgnoreCase)<0) 
            {
                return; 
            }
            _allObservDataReceived.OnNext(ChunknResponse);
            OnAllEventDataReceived(ChunknResponse);
            MeaasgeResponse meaasgeResponseBase = new MeaasgeResponse();
            meaasgeResponseBase.Event=Eventtype;
           
            string chunkjson = CommonHelper.RemoveDataPrefix(ChunknResponse);
            meaasgeResponseBase.RealJsonstring = chunkjson;
            if (UseUnsafeRelaxedJsonEscaping)
            {
                chunkjson = CommonHelper.DecodeJson(chunkjson);
            }
            try
            {
                JsonNode jsonNode = JsonNode.Parse(chunkjson)!;
                string content = jsonNode!["content"]!.GetValue<string>();
                meaasgeResponseBase.content = content;

            }
            catch (Exception)
            {

                //meaasgeResponseBase.RealJsonstring = chunkjson;
                meaasgeResponseBase.content = chunkjson;
            }







            _observMessageReceived.OnNext(meaasgeResponseBase);
            OnEventMessageReceived(meaasgeResponseBase);

        }
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
        /// <param name="body_UrlEncoded"></param>
        /// <param name="postData"></param>
        /// <param name="streamResponse"></param>
        /// <param name="ApiType"></param>
        /// <param name="UseAuthHeadersToken"></param> 
        /// <returns></returns>
        public virtual async Task<Byte[]> SendRequestAsByteAsync(HttpMethod method, string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, List<KeyValuePair<string, string>>? body_UrlEncoded = null,object ? postData = null, bool streamResponse = false, string ApiType = "", bool UseAuthHeadersToken=true)
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

                /*鉴权*/

                if (UseAuthHeadersToken)
                {

                
                Dictionary<string, string> dicAuthHeaders = GetApiAuthHeadersByToken();
                foreach (var item in dicAuthHeaders)
                {
                    request.Headers.Add(item.Key, item.Value);
                }

                }

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


                if (body_UrlEncoded!=null)
                {
                    // request.Content = this.Body_UrlEncoded;
                    FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(body_UrlEncoded!);
                    request.Content = formUrlEncodedContent;
                   // string debugContent = await request.Content.ReadAsStringAsync();
                }
                //try
                //{

                // Send the request and get the response
                // using (var response = await httpClient.SendAsync(request, streamResponse ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead))
                
                using (var response = await httpClient.SendAsync(request, streamResponse ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead))
                {
                    //string debugContent = await request.Content.ReadAsStringAsync();
                    // response.EnsureSuccessStatusCode();

                    if (streamResponse)
                    {
                        StringBuilder streamcontent = new StringBuilder();
                        // Get the response stream
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        using (var streamReader = new System.IO.StreamReader(responseStream, Encoding.UTF8))
                        {
                            string? line;
                            string eventtype=string.Empty;
                            while ((line = await streamReader.ReadLineAsync()) != null)
                            {
                                streamcontent.AppendLine(line);

                                if (UseUnsafeRelaxedJsonEscaping)
                                {

                                    string resline = CommonHelper.DecodeJson(line);
                                    line = resline;


                                }
                                if (line.IndexOf("event", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    eventtype = CommonHelper.RemovePrefixBykey(line, "event:");
                                }
                                //if (line.IndexOf("data", StringComparison.OrdinalIgnoreCase) >= 0)
                                else
                                {
                                    ProcessServerSentEventsData(line, ApiType, eventtype);
                                }
                               

                            }
                            #region SSE结束标记


                            // SSE结束标记
                            try
                            {
                                MeaasgeResponse meaasgeResponseBase = new MeaasgeResponse();
                                meaasgeResponseBase.IsCompletedSSE = true;
                                _observMessageReceived.OnNext(meaasgeResponseBase);
                                OnEventMessageReceived(meaasgeResponseBase);

                                /*observ标记完成并且重新初始化*/
                                _observMessageReceived.OnCompleted();
                                _observMessageReceived = new Subject<MeaasgeResponse>();
                            }
                            catch (Exception)
                            {

                                // throw;
                            }
                            #endregion


                        }

                        // content = streamcontent.ToString();
                        httpResponseMessage = response;
                        return EncodingDefault.GetBytes(streamcontent.ToString());
                    }
                    else
                    {
                        // Get the full response content
                        byte[] responseByte = await response.Content.ReadAsByteArrayAsync();
                        httpResponseMessage = response;
                        return responseByte;


                    }

                }

                //using (var response = await httpClient.SendAsync(request))
                //{
                //    // response.EnsureSuccessStatusCode();
                //    // Get the full response content
                //    byte[] responseByte = await response.Content.ReadAsByteArrayAsync();
                //    httpResponseMessage = response;
                //    return responseByte;

                //}

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
        /// <param name="body_UrlEncoded"></param>
        /// <param name="postData"></param>
        /// <param name="streamResponse"></param>
        /// <param name="ApiType"></param>
        ///  <param name="UseAuthHeadersToken"></param>
        /// <returns></returns>
        public virtual async Task<string> SendRequestAsync(HttpMethod method, string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, List<KeyValuePair<string, string>>? body_UrlEncoded=null, object? postData = null, bool streamResponse = false, string ApiType = "", bool UseAuthHeadersToken = true)
        {

            string responsecontent = string.Empty;
            try
            {
                Byte[] bytes = await SendRequestAsByteAsync(method: method, url: url, queryParameters: queryParameters, requestheaders: requestheaders, multipartFormData: multipartFormData,body_UrlEncoded: body_UrlEncoded, postData: postData, streamResponse: streamResponse, ApiType: ApiType, UseAuthHeadersToken: UseAuthHeadersToken);
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
        /// <param name="streamResponse"></param>
        /// <param name="ApiType"></param>
        /// <returns></returns>
        public virtual async Task<string> SendRequestByFormDataAsync(string url, Dictionary<string, string>? queryParameters = null, Dictionary<string, string>? requestheaders = null, MultipartFormDataContent? multipartFormData = null, object? postData = null, bool streamResponse = false, string ApiType = "")
        {
            // method = method ?? HttpMethod.Post;
            return await SendRequestAsync(HttpMethod.Post, url: url, queryParameters: queryParameters, requestheaders: requestheaders, multipartFormData: multipartFormData,postData: postData, streamResponse: streamResponse, ApiType: ApiType);
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

        #region disposed释放

        // IDisposable implementation
        private bool disposed = false;
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {

                if (disposing)
                {
                    try
                    {
                        // 清空所有字段
                        Url = string.Empty;
                        ApiKey = string.Empty;
                        SecretKey = string.Empty;
                        Instance = null;
                        RealRequestBody = string.Empty;
                        useUnsafeRelaxedJsonEscaping = false;
                        if (httpClient != null)
                        {
                            httpClient.Dispose();
                        }

                    }
                    catch (Exception)
                    {

                        // throw;
                    }



                }
                // 释放非托管资源（如果有的话）
                disposed = true;
            }
        }
        /// <summary>
        /// ~符号用于定义类的析构函数（destructor）,当垃圾回收器（garbage collector）决定释放对象时，会调用这个方法。析构函数用于执行清理操作
        /// </summary>
        ~AipptClientBase()
        {
            Dispose(false);
        }
        #endregion
    }
}
