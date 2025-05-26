using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic; // 引入 List<T> 所需的命名空间
using System.Text.Json.Nodes;
using System;
using System.Text.Json.Serialization;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 下载导出文件响应类，包含返回码、数据（导出的作品链接列表）和提示信息
    /// Download export file response class, containing return code, data (list of exported work links), and prompt information
    /// </summary>
    public class DownloadExportFileResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 导出的作品链接
        /// Exported work links
        /// </summary>
        public List<string>? data { get; set; } = new List<string>(); // 初始化列表以避免 null 引用

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
                    DeserializeDownloadExportFileResponse(value!);
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
        public virtual void DeserializeDownloadExportFileResponse(string json)
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

                try
                {
                    var dataNode = jsonNode["data"]?.AsArray(); // 将 data 节点解析为 JsonArray
                    if (dataNode != null)
                    {
                        this.data = new List<string>(); // 确保 data 列表已初始化
                        foreach (var item in dataNode)
                        {
                            if (item != null)
                            {
                                this.data.Add(item.GetValue<string>());
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
}
