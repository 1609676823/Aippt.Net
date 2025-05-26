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
    /// 设计保存响应类
    /// Design Save Response Class
    /// </summary>
    public class DesignSaveResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception.
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含设计保存相关数据
        /// Object containing design save related data.
        /// </summary>
        public DesignSaveData? data { get; set; } = new DesignSaveData();

        /// <summary>
        /// 返回提示信息
        /// Return prompt information.
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
                    DeserializeDesignSaveResponse(value!);
                }
                catch (Exception)
                {
                    // Optionally log the exception
                }
            }
        }

        /// <summary>
        /// 反序列化 JSON 字符串到当前的实例。
        /// 该方法会解析 JSON 中的每个属性，并将值赋给当前实例的对应属性。
        /// 如果在解析过程中出现异常，会捕获异常并继续解析后续属性。
        /// Deserializes a JSON string into the current instance.
        /// This method parses each property in the JSON and assigns the values to the corresponding properties of the current instance.
        /// If an exception occurs during the parsing process, it catches the exception and continues to parse the subsequent properties.
        /// </summary>
        /// <param name="json">需要解析的 JSON 字符串。The JSON string to be parsed.</param>
        public virtual void DeserializeDesignSaveResponse(string json)
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
                    this.data = new DesignSaveData();

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
                        var sizeNode = dataNode["size"];
                        if (sizeNode != null)
                        {
                            this.data.size = sizeNode.GetValue<string>();
                        }
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// 设计保存数据类
    /// Design Save Data Class
    /// </summary>
    public class DesignSaveData
    {
        /// <summary>
        /// 主键ID，后续接口传参用到的 user_design_id
        /// Primary key ID, used as user_design_id in subsequent API calls.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 作品名称
        /// Name of the design.
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 封面图文件地址
        /// File address of the cover image.
        /// </summary>
        public string? cover_url { get; set; }

        /// <summary>
        /// 画布大小
        /// Canvas size.
        /// </summary>
        public string? size { get; set; }
    }
}
