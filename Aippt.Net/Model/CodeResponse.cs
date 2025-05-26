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
    /// 响应类，包含返回码、数据和提示信息
    /// Response class, containing return code, data, and prompt information
    /// </summary>
    public class CodeResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含数据详情的对象
        /// Object containing data details
        /// </summary>
        public CodeData? data { get; set; } = new CodeData(); // 初始化，与TokenResponse保持一致

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
                    // 调用自定义的反序列化方法
                    DeserializeCodeResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
                    // Optional: Add logging or other error handling
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
        public virtual void DeserializeCodeResponse(string json)
        {
            var jsonNode = JsonNode.Parse(json);
            if (jsonNode != null)
            {
                // 尝试解析 code 属性 (顶层)
                try
                {
                    var topLevelCodeNode = jsonNode["code"]; // 使用更清晰的变量名
                    if (topLevelCodeNode != null)
                    {
                        this.code = topLevelCodeNode.GetValue<long>();
                    }
                }
                catch { } // 忽略 code 解析中的异常

                // 尝试解析 msg 属性
                try
                {
                    var msgNode = jsonNode["msg"];
                    if (msgNode != null)
                    {
                        this.msg = msgNode.GetValue<string>();
                    }
                }
                catch { } // 忽略 msg 解析中的异常

                // 尝试解析 data 对象及其属性
                var dataNode = jsonNode["data"];
                if (dataNode != null)
                {
                    this.data = new CodeData(); // 确保 data 对象存在

                    try
                    {
                        var apiKeyNode = dataNode["api_key"];
                        if (apiKeyNode != null)
                        {
                            this.data.api_key = apiKeyNode.GetValue<string>();
                        }
                    }
                    catch { } // 忽略 api_key 解析中的异常

                    try
                    {
                        var uidNode = dataNode["uid"];
                        if (uidNode != null)
                        {
                            this.data.uid = uidNode.GetValue<string>();
                        }
                    }
                    catch { } // 忽略 uid 解析中的异常

                    try
                    {
                        // 修正：这里是解析 data 内部的 code 属性
                        var dataCodeNode = dataNode["code"]; // 正确的变量名
                        if (dataCodeNode != null)
                        {
                            this.data.code = dataCodeNode.GetValue<string>();
                        }
                    }
                    catch { } // 忽略 data.code 解析中的异常

                    try
                    {
                        var timeExpireNode = dataNode["time_expire"];
                        if (timeExpireNode != null)
                        {
                            this.data.time_expire = timeExpireNode.GetValue<long>();
                        }
                    }
                    catch { } // 忽略 time_expire 解析中的异常
                }
            }
        }
    }

    /// <summary>
    /// 数据详情类
    /// Data details class
    /// </summary>
    public class CodeData
    {
        /// <summary>
        /// 创建账号时生成的 API Key
        /// API key generated when creating an account
        /// </summary>
        public string? api_key { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// Unique user identifier
        /// </summary>
        public string? uid { get; set; }

        /// <summary>
        /// 某种代码或标识符 (根据JSON结构判断)
        /// Some code or identifier (judging from the JSON structure)
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 有效期
        /// Validity period
        /// </summary>
        public long time_expire { get; set; }
    }
}
