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
    /// Task creation response class, containing return code, data, and prompt information
    /// 任务创建响应类，包含返回码、数据和提示信息
    /// </summary>
    public class TaskCreateResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含任务创建相关数据的对象
        /// Object containing task creation related data
        /// </summary>
        public TaskCreateData? data { get; set; } = new TaskCreateData();

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
                    DeserializeTaskCreateResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
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
        public virtual void DeserializeTaskCreateResponse(string json)
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
                    this.data = new TaskCreateData();

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
                        var typeNode = dataNode["type"];
                        if (typeNode != null)
                        {
                            this.data.type = typeNode.GetValue<long>();
                        }
                    }
                    catch { }

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
                        var createdAtNode = dataNode["created_at"];
                        if (createdAtNode != null)
                        {
                            this.data.created_at = createdAtNode.GetValue<string>();
                        }
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// Task data class, containing information such as ID, title, type, API key, and creation time
    /// 任务数据类，包含 ID、标题、类型、API 密钥和创建时间等信息
    /// </summary>
    public class TaskCreateData
    {
        /// <summary>
        /// 任务的唯一标识
        /// Unique identifier of the task
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 任务标题
        /// Task title
        /// </summary>
        public string? title { get; set; }

        /// <summary>
        /// 任务类型
        /// Task type
        /// </summary>
        public long type { get; set; }

        /// <summary>
        /// 创建任务时使用的 API Key
        /// API key used when creating the task
        /// </summary>
        public string? api_key { get; set; }

        /// <summary>
        /// 任务创建时间
        /// Task creation time
        /// </summary>
        public string? created_at { get; set; }
    }
}