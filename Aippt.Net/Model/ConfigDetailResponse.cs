using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 配置详情响应类，包含返回码、数据（配置详情）和提示信息
    /// Configuration detail response class, containing return code, data (configuration details), and prompt information
    /// </summary>
    public class ConfigDetailResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含配置详情的数据对象
        /// Data object containing configuration details
        /// </summary>
        public ConfigDetailData? data { get; set; } = new ConfigDetailData();

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
                    DeserializeConfigDetailResponse(value!);
                }
                catch (Exception)
                {
                    // 异常处理，可根据需要记录日志
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
        public virtual void DeserializeConfigDetailResponse(string json)
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
                    this.data = new ConfigDetailData();

                    try
                    {
                        var idNode = dataNode["id"];
                        if (idNode != null)
                        {
                            this.data.id = idNode.GetValue<long>();
                        }
                    }
                    catch { }

                    try
                    {
                        var titleNode = dataNode["title"];
                        if (titleNode != null)
                        {
                            this.data.title = titleNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var sourceNode = dataNode["source"];
                        if (sourceNode != null)
                        {
                            this.data.source = sourceNode.GetValue<long>();
                        }
                    }
                    catch { }

                    try
                    {
                        var contentNode = dataNode["content"];
                        if (contentNode != null)
                        {
                            this.data.content = contentNode.GetValue<string>();
                        }
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// 配置详情数据类，包含预置词ID、标题、来源和内容
    /// Configuration detail data class, containing preset word ID, title, source, and content
    /// </summary>
    public class ConfigDetailData
    {
        /// <summary>
        /// 预置词ID
        /// Preset word ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 预置词标题
        /// Preset word title
        /// </summary>
        public string? title { get; set; }

        /// <summary>
        /// 预置词来源
        /// Preset word source
        /// </summary>
        public long source { get; set; }

        /// <summary>
        /// 预置词内容（通常为 Markdown 格式）
        /// Preset word content (usually in Markdown format)
        /// </summary>
        public string? content { get; set; }
    }
}