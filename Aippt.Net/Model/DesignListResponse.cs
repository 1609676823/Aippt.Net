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
    /// 设计列表响应类
    /// Design List Response Class
    /// </summary>
    public class DesignListResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception.
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含设计列表相关数据
        /// Object containing design list related data.
        /// </summary>
        public DesignListData? data { get; set; } = new DesignListData();

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
                    DeserializeDesignListResponse(value!);
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
        public virtual void DeserializeDesignListResponse(string json)
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
                    this.data = new DesignListData();

                    // Deserialize pagination
                    var paginationNode = dataNode["pagination"];
                    if (paginationNode != null)
                    {
                        this.data.DesignListPagination = new DesignListPagination();
                        try
                        {
                            var totalNode = paginationNode["total"];
                            if (totalNode != null)
                            {
                                this.data.DesignListPagination.total = totalNode.GetValue<long>();
                            }
                        }
                        catch { }
                        try
                        {
                            var currentPageNode = paginationNode["current_page"];
                            if (currentPageNode != null)
                            {
                                this.data.DesignListPagination.current_page = currentPageNode.GetValue<long>();
                            }
                        }
                        catch { }
                        try
                        {
                            var pageSizeNode = paginationNode["page_size"];
                            if (pageSizeNode != null)
                            {
                                this.data.DesignListPagination.page_size = pageSizeNode.GetValue<long>();
                            }
                        }
                        catch { }
                    }

                    // Deserialize list of DesignListItem
                    var listNode = dataNode["list"];
                    if (listNode != null && listNode.AsArray() != null)
                    {
                        this.data.list = new List<DesignListItem>();
                        foreach (var itemNode in listNode.AsArray())
                        {
                            if (itemNode != null)
                            {
                                var designListItem = new DesignListItem();
                                try
                                {
                                    var idNode = itemNode["id"];
                                    if (idNode != null)
                                    {
                                        designListItem.id = idNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var userIdNode = itemNode["user_id"];
                                    if (userIdNode != null)
                                    {
                                        designListItem.user_id = userIdNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var nameNode = itemNode["name"];
                                    if (nameNode != null)
                                    {
                                        designListItem.name = nameNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var coverUrlNode = itemNode["cover_url"];
                                    if (coverUrlNode != null)
                                    {
                                        designListItem.cover_url = coverUrlNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var sizeNode = itemNode["size"];
                                    if (sizeNode != null)
                                    {
                                        designListItem.size = sizeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var canvasUrlNode = itemNode["canvas_url"];
                                    if (canvasUrlNode != null)
                                    {
                                        designListItem.canvas_url = canvasUrlNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var aiDataNode = itemNode["ai_data"];
                                    if (aiDataNode != null)
                                    {
                                        designListItem.ai_data = aiDataNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var storageTimeNode = itemNode["storage_time"];
                                    if (storageTimeNode != null)
                                    {
                                        designListItem.storage_time = storageTimeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var deleteTimeNode = itemNode["delete_time"];
                                    if (deleteTimeNode != null)
                                    {
                                        designListItem.delete_time = deleteTimeNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var autoDeleteTimeNode = itemNode["auto_delete_time"];
                                    if (autoDeleteTimeNode != null)
                                    {
                                        designListItem.auto_delete_time = autoDeleteTimeNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var createdAtNode = itemNode["created_at"];
                                    if (createdAtNode != null)
                                    {
                                        designListItem.created_at = createdAtNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var updatedAtNode = itemNode["updated_at"];
                                    if (updatedAtNode != null)
                                    {
                                        designListItem.updated_at = updatedAtNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var templateTypeNode = itemNode["template_type"];
                                    if (templateTypeNode != null)
                                    {
                                        designListItem.template_type = templateTypeNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var localPptUploadIdNode = itemNode["local_ppt_upload_id"];
                                    if (localPptUploadIdNode != null)
                                    {
                                        designListItem.local_ppt_upload_id = localPptUploadIdNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var userTemplateIdNode = itemNode["user_template_id"];
                                    if (userTemplateIdNode != null)
                                    {
                                        designListItem.user_template_id = userTemplateIdNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var userDesignIdNode = itemNode["user_design_id"];
                                    if (userDesignIdNode != null)
                                    {
                                        designListItem.user_design_id = userDesignIdNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var userDesignTypeNode = itemNode["user_design_type"];
                                    if (userDesignTypeNode != null)
                                    {
                                        designListItem.user_design_type = userDesignTypeNode.GetValue<long>();
                                    }
                                }
                                catch { }
                                try
                                {
                                    var stagedCanvasUrlNode = itemNode["staged_canvas_url"];
                                    if (stagedCanvasUrlNode != null)
                                    {
                                        designListItem.staged_canvas_url = stagedCanvasUrlNode.GetValue<string>();
                                    }
                                }
                                catch { }
                                this.data.list.Add(designListItem);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设计列表数据类，包含分页信息和设计项目列表。
    /// Design list data class, containing pagination information and a list of design items.
    /// </summary>
    public class DesignListData
    {
        /// <summary>
        /// 分页信息
        /// Pagination information.
        /// </summary>
        public DesignListPagination? DesignListPagination { get; set; } = new DesignListPagination();

        /// <summary>
        /// 设计项目列表
        /// List of design items.
        /// </summary>
        public List<DesignListItem>? list { get; set; } = new List<DesignListItem>();
    }

    /// <summary>
    /// 设计列表分页信息类
    /// Design List Pagination Information Class.
    /// </summary>
    public class DesignListPagination
    {
        /// <summary>
        /// 总条数
        /// Total number of records.
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 当前页
        /// Current page number.
        /// </summary>
        public long current_page { get; set; }

        /// <summary>
        /// 每页条数
        /// Number of items per page.
        /// </summary>
        public long page_size { get; set; }
    }

    /// <summary>
    /// 单个设计列表项目类
    /// Single Design List Item Class.
    /// </summary>
    public class DesignListItem
    {
        /// <summary>
        /// 主键ID，后续接口传参用到的 user_design_id
        /// Primary key ID, used as user_design_id in subsequent API calls.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 用户ID
        /// User ID.
        /// </summary>
        public long user_id { get; set; }

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

        /// <summary>
        /// 画布json地址
        /// Canvas JSON file address.
        /// </summary>
        public string? canvas_url { get; set; }

        /// <summary>
        /// AI文案文件地址
        /// AI copy file address.
        /// </summary>
        public string? ai_data { get; set; }

        /// <summary>
        /// 保存时间
        /// Storage time.
        /// </summary>
        public string? storage_time { get; set; }

        /// <summary>
        /// 删除时间
        /// Delete time.
        /// </summary>
        public string? delete_time { get; set; }

        /// <summary>
        /// 自动删除时间
        /// Auto-delete time.
        /// </summary>
        public long auto_delete_time { get; set; }

        /// <summary>
        /// 创建时间
        /// Creation time.
        /// </summary>
        public string? created_at { get; set; }

        /// <summary>
        /// 更新时间
        /// Update time.
        /// </summary>
        public string? updated_at { get; set; }

        /// <summary>
        /// 模板类型
        /// Template type.
        /// </summary>
        public long template_type { get; set; }

        /// <summary>
        /// 本地PPT上传ID
        /// Local PPT upload ID.
        /// </summary>
        public long local_ppt_upload_id { get; set; }

        /// <summary>
        /// 用户模板ID
        /// User template ID.
        /// </summary>
        public long user_template_id { get; set; }

        /// <summary>
        /// 用户设计ID
        /// User design ID.
        /// </summary>
        public long user_design_id { get; set; }

        /// <summary>
        /// 用户设计类型
        /// User design type.
        /// </summary>
        public long user_design_type { get; set; }

        /// <summary>
        /// 暂存画布URL
        /// Staged canvas URL.
        /// </summary>
        public string? staged_canvas_url { get; set; }
    }
}


