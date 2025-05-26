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
    /// 配置列表响应类，包含返回码、数据（分页信息和配置项列表）和提示信息
    /// Configuration list response class, containing return code, data (pagination information and configuration item list), and prompt information
    /// </summary>
    public class ConfigListResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含分页信息和配置项列表的数据对象
        /// Data object containing pagination information and configuration item list
        /// </summary>
        public ConfigListData? data { get; set; } = new ConfigListData();

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
                    DeserializeConfigListResponse(value!);
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
        public virtual void DeserializeConfigListResponse(string json)
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
                    this.data = new ConfigListData();

                    // 解析 pagination
                    var paginationNode = dataNode["pagination"];
                    if (paginationNode != null)
                    {
                        this.data.pagination = new ConfigListPagination();
                        try
                        {
                            var totalNode = paginationNode["total"];
                            if (totalNode != null)
                            {
                                this.data.pagination.total = totalNode.GetValue<long>();
                            }
                        }
                        catch { }
                        try
                        {
                            var currentPageNode = paginationNode["current_page"];
                            if (currentPageNode != null)
                            {
                                this.data.pagination.current_page = currentPageNode.GetValue<long>();
                            }
                        }
                        catch { }
                        try
                        {
                            var pageSizeNode = paginationNode["page_size"];
                            if (pageSizeNode != null)
                            {
                                this.data.pagination.page_size = pageSizeNode.GetValue<long>();
                            }
                        }
                        catch { }
                    }

                    // 解析 list
                    var listNode = dataNode["list"]?.AsArray();
                    if (listNode != null)
                    {
                        this.data.list = new List<ConfigListItem>();
                        foreach (var item in listNode)
                        {
                            if (item != null)
                            {
                                var listItem = new ConfigListItem();
                                try
                                {
                                    var idNode = item["id"];
                                    if (idNode != null)
                                    {
                                        listItem.id = idNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var titleNode = item["title"];
                                    if (titleNode != null)
                                    {
                                        listItem.title = titleNode.GetValue<string>();
                                    }
                                }
                                catch { }

                                this.data.list.Add(listItem);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 配置列表数据类，包含分页信息和配置项列表
    /// Configuration list data class, containing pagination information and configuration item list
    /// </summary>
    public class ConfigListData
    {
        /// <summary>
        /// 分页信息
        /// Pagination information
        /// </summary>
        public ConfigListPagination? pagination { get; set; } = new ConfigListPagination();

        /// <summary>
        /// 配置项列表
        /// List of configuration items
        /// </summary>
        public List<ConfigListItem>? list { get; set; } = new List<ConfigListItem>();
    }

    /// <summary>
    /// 配置列表分页信息类
    /// Configuration list pagination information class
    /// </summary>
    public class ConfigListPagination
    {
        /// <summary>
        /// 总条数
        /// Total count of items
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 当前页
        /// Current page number
        /// </summary>
        public long current_page { get; set; }

        /// <summary>
        /// 每页条数
        /// Number of items per page
        /// </summary>
        public long page_size { get; set; }
    }

    /// <summary>
    /// 配置列表项类
    /// Configuration list item class
    /// </summary>
    public class ConfigListItem
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
    }
}
