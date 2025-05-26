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
    /// Suit search response class, containing return code, data, and prompt information.
    /// </summary>
    public class SuitSearchResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含套装搜索结果数据的对象
        /// Object containing suit search result data
        /// </summary>
        public SuitSearchData? data { get; set; } = new SuitSearchData();

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
                    DeserializeSuitSearchResponse(value!);
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
        public virtual void DeserializeSuitSearchResponse(string json)
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
                    this.data = new SuitSearchData();

                    try
                    {
                        var paginationNode = dataNode["pagination"];
                        if (paginationNode != null)
                        {
                            this.data.pagination = new Pagination();
                            var totalNode = paginationNode["total"];
                            if (totalNode != null)
                            {
                                this.data.pagination.total = totalNode.GetValue<long>();
                            }
                            var currentPageNode = paginationNode["current_page"];
                            if (currentPageNode != null)
                            {
                                this.data.pagination.current_page = currentPageNode.GetValue<long>();
                            }
                            var pageSizeNode = paginationNode["page_size"];
                            if (pageSizeNode != null)
                            {
                                this.data.pagination.page_size = pageSizeNode.GetValue<long>();
                            }
                        }
                    }
                    catch { }

                    try
                    {
                        var listNode = dataNode["list"]!.AsArray();
                        if (listNode != null)
                        {
                            this.data.list = new List<SuitItem>();
                            foreach (var itemNode in listNode)
                            {
                                SuitItem item = new SuitItem();
                                var idNode = itemNode!["id"];
                                if (idNode != null)
                                {
                                    item.id = idNode.GetValue<long>();
                                }
                                var coverImgNode = itemNode!["cover_img"];
                                if (coverImgNode != null)
                                {
                                    item.cover_img = coverImgNode.GetValue<string>();
                                }
                                this.data.list.Add(item);
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// 套装搜索数据类，包含分页信息和套装列表
    /// Suit search data class, containing pagination information and suit list
    /// </summary>
    public class SuitSearchData
    {
        /// <summary>
        /// 分页信息
        /// Pagination information
        /// </summary>
        public Pagination? pagination { get; set; } = new Pagination();

        /// <summary>
        /// 套装列表
        /// List of suits
        /// </summary>
        public List<SuitItem>? list { get; set; } = new List<SuitItem>();
    }

    /// <summary>
    /// 分页信息类
    /// Pagination information class
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 总数
        /// Total count
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 当前页码
        /// Current page number
        /// </summary>
        public long current_page { get; set; }

        /// <summary>
        /// 每页大小
        /// Page size
        /// </summary>
        public long page_size { get; set; }
    }

    /// <summary>
    /// 套装项类
    /// Suit item class
    /// </summary>
    public class SuitItem
    {
        /// <summary>
        /// 套装ID
        /// Suit ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 封面图片URL
        /// Cover image URL
        /// </summary>
        public string? cover_img { get; set; }
    }
}
