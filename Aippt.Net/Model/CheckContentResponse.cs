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
    /// 内容检查响应类，包含返回码、数据和提示信息
    /// Content check response class, containing return code, data, and prompt information
    /// </summary>
    public class CheckContentResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含内容检查相关数据的对象
        /// Object containing content check related data
        /// </summary>
        public CheckContentData? data { get; set; } = new CheckContentData();

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
                    DeserializeCheckContentResponse(value!);
                }
                catch (Exception)
                {
                    // Log the exception or handle it as needed
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
        public virtual void DeserializeCheckContentResponse(string json)
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
                catch { } // Catch exception for code property parsing

                try
                {
                    var msgNode = jsonNode["msg"];
                    if (msgNode != null)
                    {
                        this.msg = msgNode.GetValue<string>();
                    }
                }
                catch { } // Catch exception for msg property parsing

                var dataNode = jsonNode["data"];
                if (dataNode != null)
                {
                    this.data = new CheckContentData();

                    try
                    {
                        var statusNode = dataNode["status"];
                        if (statusNode != null)
                        {
                            this.data.status = statusNode.GetValue<int>();
                        }
                    }
                    catch { } // Catch exception for data.status property parsing

                    try
                    {
                        var contentNode = dataNode["content"];
                        if (contentNode != null)
                        {
                            this.data.content = contentNode.GetValue<string>();
                        }
                    }
                    catch { } // Catch exception for data.content property parsing
                }
            }
        }
    }

    /// <summary>
    /// 内容检查数据类，包含状态和内容信息
    /// Content check data class, containing status and content information
    /// </summary>
    public class CheckContentData
    {
        /// <summary>
        /// 内容生成状态，1表示生成中，2表示完成
        /// Content generation status, 1 indicates in progress, 2 indicates complete
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// status为2时，content值为大纲内容
        /// When status is 2, the content value is the outline content
        /// </summary>
        public string? content { get; set; }
    }
}
