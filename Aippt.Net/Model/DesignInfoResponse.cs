using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes; // 请确保您已引用 System.Text.Json 命名空间
using System.Text.Json.Serialization; // 用于 [JsonIgnore] 属性

namespace Aippt.Net.Model
{
    /// <summary>
    /// 设计信息响应类，包含返回码、数据和提示信息
    /// Design information response class, containing return code, data, and prompt information
    /// </summary>
    public class DesignInfoResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含设计信息相关数据的对象
        /// Object containing design-related data
        /// </summary>
        public DesignInfoData? data { get; set; } = new DesignInfoData();

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
                    DeserializeDesignInfoResponse(value!);
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
        public virtual void DeserializeDesignInfoResponse(string json)
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
                    this.data = new DesignInfoData();

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
                        var nameNode = dataNode["name"];
                        if (nameNode != null)
                        {
                            this.data.name = nameNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var coverUrlNode = dataNode["cover_url"];
                        if (coverUrlNode != null)
                        {
                            this.data.cover_url = coverUrlNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var taskIdNode = dataNode["task_id"];
                        if (taskIdNode != null)
                        {
                            this.data.task_id = taskIdNode.GetValue<long>();
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

                    try
                    {
                        var storageTimeNode = dataNode["storage_time"];
                        if (storageTimeNode != null)
                        {
                            this.data.storage_time = storageTimeNode.GetValue<string>();
                        }
                    }
                    catch { }

                    try
                    {
                        var versionNode = dataNode["version"];
                        if (versionNode != null)
                        {
                            this.data.version = versionNode.GetValue<long>();
                        }
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// 设计信息数据类
    /// Design information data class
    /// </summary>
    public class DesignInfoData
    {
        /// <summary>
        /// 作品ID
        /// Work ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 作品名称
        /// Work name
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 封面图文件地址
        /// Cover image file address
        /// </summary>
        public string? cover_url { get; set; }

        /// <summary>
        /// 任务ID
        /// Task ID
        /// </summary>
        public long task_id { get; set; }

        /// <summary>
        /// 创建时间
        /// Creation time
        /// </summary>
        public string? created_at { get; set; }

        /// <summary>
        /// 保存时间
        /// Save time
        /// </summary>
        public string? storage_time { get; set; }

        /// <summary>
        /// 作品版本号
        /// Work version number
        /// </summary>
        public long version { get; set; }
    }
}
