using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 高级选项响应类，包含返回码、数据列表和提示信息
    /// Senior option response class, containing return code, data list, and prompt information
    /// </summary>
    public class SeniorOptionResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含高级选项数据的列表
        /// List containing senior option data
        /// </summary>
        public List<SeniorOptionData>? data { get; set; }

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
                    // Using the provided Deserialize pattern
                    DeserializeSeniorOptionResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
                    // Add logging or other operations here
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
        public virtual void DeserializeSeniorOptionResponse(string json)
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
                if (dataNode != null && dataNode is JsonArray jsonArray)
                {
                    this.data = new List<SeniorOptionData>();
                    foreach (var element in jsonArray)
                    {
                        if (element != null)
                        {
                            // Use JsonSerializer for nested object deserialization for simplicity
                            // or implement manual parsing like the TokenResponse class (more complex for recursive)
                            try
                            {
                                var optionData = System.Text.Json.JsonSerializer.Deserialize<SeniorOptionData>(element.ToJsonString());
                                if (optionData != null)
                                {
                                    this.data.Add(optionData);
                                }
                            }
                            catch { } // Catch exceptions for each data item
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 高级选项数据类，包含选项的ID、父ID、名称、键、默认状态、Beta状态和子选项列表
    /// Senior option data class, containing information such as option ID, parent ID, name, key, default status, beta status, and list of child options
    /// </summary>
    public class SeniorOptionData
    {
        /// <summary>
        /// 选项ID
        /// Option ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 选项父级ID
        /// Option parent ID
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// 选项名称
        /// Option name
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 选项key
        /// Option key
        /// </summary>
        public string? key { get; set; }

        /// <summary>
        /// 是否默认选项，0表示否，1表示是
        /// Whether it is the default option, 0 for no, 1 for yes
        /// </summary>
        public int is_default { get; set; }

        /// <summary>
        /// 是否beta版，0表示否，1表示是
        /// Whether it is a Beta version, 0 for no, 1 for yes
        /// </summary>
        public int is_beta { get; set; }

        /// <summary>
        /// 子级数组
        /// Child array
        /// </summary>
        public List<SeniorOptionData>? children { get; set; }
    }
}
