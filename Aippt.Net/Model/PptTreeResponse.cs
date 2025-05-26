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
    /// PPT树形结构响应类，包含返回码、数据和提示信息
    /// PPT tree structure response class, containing return code, data, and prompt information
    /// </summary>
    public class PptTreeResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含PPT树形结构数据的对象
        /// Object containing PPT tree structure data
        /// </summary>
        public PptTreeData? data { get; set; } = new PptTreeData();

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
                    DeserializePptTreeResponse(value!);
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
        public virtual void DeserializePptTreeResponse(string json)
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
                    this.data = new PptTreeData();
                    // Deserialize children recursively
                    this.data.DeserializePptTreeNode(dataNode);
                }
            }
        }
    }

    /// <summary>
    /// PPT树形结构数据类，包含PPT树的根节点信息
    /// PPT tree structure data class, containing root node information of the PPT tree
    /// </summary>
    public class PptTreeData : PptTreeNode
    {
        // PptTreeData 继承自 PptTreeNode，所以它本身就包含了根节点的属性
    }

    /// <summary>
    /// PPT树形结构节点类，代表树中的一个节点
    /// PPT tree structure node class, representing a node in the tree
    /// </summary>
    public class PptTreeNode
    {
        /// <summary>
        /// 子节点列表
        /// List of child nodes
        /// </summary>
        public List<PptTreeNode>? children { get; set; } = new List<PptTreeNode>();

        /// <summary>
        /// 节点深度
        /// Node depth
        /// </summary>
        public long depth { get; set; }

        /// <summary>
        /// 方向
        /// Direction
        /// </summary>
        public long direction { get; set; }

        /// <summary>
        /// 是否展开
        /// Whether expanded
        /// </summary>
        public bool expanded { get; set; }

        /// <summary>
        /// 节点ID
        /// Node ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 索引
        /// Index
        /// </summary>
        public long? index { get; set; }

        /// <summary>
        /// 是否是最后一级
        /// Whether it's the last level
        /// </summary>
        public bool? lastLevel { get; set; }

        /// <summary>
        /// 页面索引
        /// Page index
        /// </summary>
        public long? pageIndex { get; set; }

        /// <summary>
        /// 父节点ID
        /// Parent node ID
        /// </summary>
        public long parentId { get; set; }

        /// <summary>
        /// 是否显示提示
        /// Whether to show tip
        /// </summary>
        public bool showTip { get; set; }

        /// <summary>
        /// 排序
        /// Sort order
        /// </summary>
        public long sort { get; set; }

        /// <summary>
        /// 节点类型 (e.g., "title", "catalog", "ending")
        /// Node type (e.g., "title", "catalog", "ending")
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// 节点值
        /// Node value
        /// </summary>
        public string? value { get; set; }

        /// <summary>
        /// 反序列化 JsonNode 到当前的 PptTreeNode 实例。
        /// Deserialize JsonNode into the current PptTreeNode instance.
        /// </summary>
        /// <param name="node">需要解析的 JsonNode。The JsonNode to be parsed.</param>
        public virtual void DeserializePptTreeNode(JsonNode node)
        {
            try { this.children = DeserializeChildren(node["children"]); } catch { }
            try { this.depth = node["depth"]!.GetValue<long>(); } catch { }
            try { this.direction = node["direction"]!.GetValue<long>(); } catch { }
            try { this.expanded = node["expanded"]!.GetValue<bool>(); } catch { }
            try { this.id = node["id"]!.GetValue<long>(); } catch { }
            try { this.index = node["index"]?.GetValue<long>(); } catch { }
            try { this.lastLevel = node["lastLevel"]?.GetValue<bool>(); } catch { }
            try { this.pageIndex = node["pageIndex"]?.GetValue<long>(); } catch { }
            try { this.parentId = node["parentId"]!.GetValue<long>(); } catch { }
            try { this.showTip = node["showTip"]!.GetValue<bool>(); } catch { }
            try { this.sort = node["sort"]!.GetValue<long>(); } catch { }
            try { this.type = node["type"]!.GetValue<string>(); } catch { }
            try { this.value = node["value"]!.GetValue<string>(); } catch { }
        }

        private List<PptTreeNode>? DeserializeChildren(JsonNode? childrenNode)
        {
            if (childrenNode is JsonArray childrenArray)
            {
                var childrenList = new List<PptTreeNode>();
                foreach (var child in childrenArray)
                {
                    if (child != null)
                    {
                        var treeNode = new PptTreeNode();
                        treeNode.DeserializePptTreeNode(child);
                        childrenList.Add(treeNode);
                    }
                }
                return childrenList;
            }
            return null;
        }
    }
}
