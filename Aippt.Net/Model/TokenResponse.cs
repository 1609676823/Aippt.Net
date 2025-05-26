using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 令牌响应类，包含返回码、数据和提示信息
    /// Token response class, containing return code, data, and prompt information
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含令牌相关数据的对象
        /// Object containing token-related data
        /// </summary>
        public TokenData? data { get; set; }=new TokenData();

        /// <summary>
        /// 返回提示信息
        /// Return prompt information
        /// </summary>
        public string? msg { get; set; }

        [JsonIgnore]
        private string realJsonstring = string.Empty;

        /// <summary>
        /// Gets or sets the real JSON string representation of the response.
        /// 获取或设置响应的真实 JSON 字符串表示。
        /// </summary>
        [JsonIgnore]
        public string? RealJsonstring
        {
            get { return realJsonstring; }
            set
            {
                realJsonstring = value!;
                try
                {
                    DeserializeTokenResponse(value!);
                }
                catch (Exception)
                {
                   
                }
            }
        }

        /// <summary>
        /// 反序列化 JSON 字符串到当前的实例。
        /// 该方法会解析 JSON 中的每个属性，并将值赋给当前实例的对应属性。
        /// 如果在解析过程中出现异常，会捕获异常并继续解析后续属性。
        /// Deserialize a JSON string into the current instance.
        /// This method parses each property in the JSON and assigns the values to the corresponding properties of the current instance.
        /// If an exception occurs during the parsing process, it catches the exception and continues to parse the subsequent properties.
        /// </summary>
        /// <param name="json">需要解析的 JSON 字符串。The JSON string to be parsed.</param>
        public virtual void DeserializeTokenResponse(string json)
        {
            var jsonNode = JsonNode.Parse(json);
            if (jsonNode != null)
            {
                try
                {
                    var codeNode = jsonNode["code"];
                    if (codeNode != null)
                    {
                        this.code = codeNode.GetValue<long>();
                    }
                }
                catch { }

                try
                {
                    var msgNode = jsonNode["msg"];
                    if (msgNode != null)
                    {
                        this.msg = msgNode.GetValue<string>();
                    }
                }
                catch { }

                var dataNode = jsonNode["data"];
                if (dataNode != null)
                {
                    this.data = new TokenData();

                    try
                    {
                        var apiKeyNode = dataNode["api_key"];
                        if (apiKeyNode != null)
                        {
                            this.data.api_key = apiKeyNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var uidNode = dataNode["uid"];
                        if (uidNode != null)
                        {
                            this.data.uid = uidNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var tokenNode = dataNode["token"];
                        if (tokenNode != null)
                        {
                            this.data.token = tokenNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var timeExpireNode = dataNode["time_expire"];
                        if (timeExpireNode != null)
                        {
                            this.data.time_expire = timeExpireNode.GetValue<long>();
                        }
                    }
                    catch { }
                }
            }
        }


    }

    /// <summary>
    /// 令牌数据类，包含 API 密钥、用户标识、令牌和有效期等信息
    /// Token data class, containing information such as API key, user ID, token, and validity period
    /// </summary>
    public class TokenData
    {
        /// <summary>
        /// 创建账号时生成的 API Key
        /// API key generated when creating an account
        /// </summary>
        public string? api_key { get; set; }

        /// <summary>
        /// 企业方自己用户的唯一标识
        /// Unique identifier of the enterprise's own user
        /// </summary>
        public string? uid { get; set; }

        /// <summary>
        /// 令牌
        /// Token
        /// </summary>
        public string? token { get; set; }

        /// <summary>
        /// 令牌有效期
        /// Token validity period
        /// </summary>
        public long time_expire { get; set; }
    }
}
