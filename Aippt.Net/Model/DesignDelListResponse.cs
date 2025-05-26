using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 作品删除列表响应类，包含返回码、数据（分页信息和作品列表）和提示信息
    /// Work deletion list response class, containing return code, data (pagination information and work list), and prompt information
    /// </summary>
    public class DesignDelListResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含分页信息和作品列表的数据对象
        /// Data object containing pagination information and work list
        /// </summary>
        public DesignDelListData? data { get; set; } = new DesignDelListData();

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
                    DeserializeDesignDelListResponse(value!);
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
        /// 如果在解析过程中出现异常，会捕复异常并继续解析后续属性。
        /// Deserialize a JSON string into the current instance.
        /// This method parses each property in the JSON and assigns the values to the corresponding properties of the current instance.
        /// If an exception occurs during the parsing process, it catches the exception and continues to parse the subsequent properties.
        /// </summary>
        /// <param name="json">需要解析的 JSON 字符串。The JSON string to be parsed.</param>
        public virtual void DeserializeDesignDelListResponse(string json)
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
                    this.data = new DesignDelListData();

                    // 解析 pagination
                    var paginationNode = dataNode["pagination"];
                    if (paginationNode != null)
                    {
                        this.data.pagination = new DesignDelListPagination();
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
                        this.data.list = new List<DesignDelListItem>();
                        foreach (var item in listNode)
                        {
                            if (item != null)
                            {
                                var listItem = new DesignDelListItem();
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
                                    var userIdNode = item["user_id"];
                                    if (userIdNode != null)
                                    {
                                        listItem.user_id = userIdNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var nameNode = item["name"];
                                    if (nameNode != null)
                                    {
                                        listItem.name = nameNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var coverUrlNode = item["cover_url"];
                                    if (coverUrlNode != null)
                                    {
                                        listItem.cover_url = coverUrlNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var sizeNode = item["size"];
                                    if (sizeNode != null)
                                    {
                                        listItem.size = sizeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var canvasUrlNode = item["canvas_url"];
                                    if (canvasUrlNode != null)
                                    {
                                        listItem.canvas_url = canvasUrlNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var aiDataNode = item["ai_data"];
                                    if (aiDataNode != null)
                                    {
                                        listItem.ai_data = aiDataNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var storageTimeNode = item["storage_time"];
                                    if (storageTimeNode != null)
                                    {
                                        listItem.storage_time = storageTimeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var deleteTimeNode = item["delete_time"];
                                    if (deleteTimeNode != null)
                                    {
                                        listItem.delete_time = deleteTimeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var autoDeleteTimeNode = item["auto_delete_time"];
                                    if (autoDeleteTimeNode != null)
                                    {
                                        listItem.auto_delete_time = autoDeleteTimeNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var createdAtNode = item["created_at"];
                                    if (createdAtNode != null)
                                    {
                                        listItem.created_at = createdAtNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var updatedAtNode = item["updated_at"];
                                    if (updatedAtNode != null)
                                    {
                                        listItem.updated_at = updatedAtNode.GetValue<string>();
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
    /// 作品删除列表数据类，包含分页信息和作品列表
    /// Work deletion list data class, containing pagination information and work list
    /// </summary>
    public class DesignDelListData
    {
        /// <summary>
        /// 分页信息
        /// Pagination information
        /// </summary>
        public DesignDelListPagination? pagination { get; set; } = new DesignDelListPagination();

        /// <summary>
        /// 作品列表
        /// Work list
        /// </summary>
        public List<DesignDelListItem>? list { get; set; } = new List<DesignDelListItem>();
    }

    /// <summary>
    /// 作品删除列表分页信息类
    /// Work deletion list pagination information class
    /// </summary>
    public class DesignDelListPagination
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
    /// 作品删除列表项类
    /// Work deletion list item class
    /// </summary>
    public class DesignDelListItem
    {
        /// <summary>
        /// 主键ID，后续接口传参用到的 user_design_id
        /// Primary key ID, used as user_design_id for subsequent API calls
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// User ID
        /// </summary>
        public long user_id { get; set; }

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
        /// 画布大小
        /// Canvas size
        /// </summary>
        public string? size { get; set; }

        /// <summary>
        /// 画布json地址
        /// Canvas JSON address
        /// </summary>
        public string? canvas_url { get; set; }

        /// <summary>
        /// AI文案文件地址
        /// AI copy file address
        /// </summary>
        public string? ai_data { get; set; }

        /// <summary>
        /// 保存时间
        /// Storage time
        /// </summary>
        public string? storage_time { get; set; }

        /// <summary>
        /// 删除时间
        /// Deletion time
        /// </summary>
        public string? delete_time { get; set; }

        /// <summary>
        /// 自动删除剩余时间（天）
        /// Remaining days until auto deletion
        /// </summary>
        public long auto_delete_time { get; set; }

        /// <summary>
        /// 创建时间
        /// Creation time
        /// </summary>
        public string? created_at { get; set; }

        /// <summary>
        /// 更新时间
        /// Update time
        /// </summary>
        public string? updated_at { get; set; }
    }
}