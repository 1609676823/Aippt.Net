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
    /// 作品删除响应类，包含返回码、数据和提示信息
    /// Work deletion response class, containing return code, data, and prompt information
    /// </summary>
    public class DesignDeleteResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 数据部分，此处通常为空数组或空对象，表示无特定返回数据
        /// Data part, usually an empty array or empty object here, indicating no specific return data
        /// </summary>
        // 虽然 JSON 中 data 是空数组，但C#中为了通用性，可以考虑用 object 或 List<object>
        // 如果确定未来 data 永远是空数组且不包含任何类型，也可以直接让其为 null，或者用 List<object>
        public object? data { get; set; } // 或者 List<object>? data { get; set; } = new List<object>();

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
                    DeserializeDesignDeleteResponse(value!);
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
        public virtual void DeserializeDesignDeleteResponse(string json)
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

                // 对于空的 JSON 数组 `[]`，可以直接让 data 保持其默认值（null 或空列表），
                // 或者根据具体业务需求进行处理。此处选择不对其进行特殊赋值。
                // 如果需要确保 data 始终是一个空列表，可以尝试：
                try
                {
                    var dataNode = jsonNode["data"];
                    if (dataNode != null)
                    {
                        // 检查是否是空数组，可以根据需要赋值
                        if (dataNode.AsArray().Count == 0)
                        {
                            this.data = new List<object>(); // 明确设置为一个空列表
                        }
                        else
                        {
                            // 如果 data 理论上可能包含其他内容，则需要进一步解析
                            // 这里我们假设它始终为空数组
                            this.data = null; // 或者保持默认值
                        }
                    }
                }
                catch { }
            }
        }
    }
}
