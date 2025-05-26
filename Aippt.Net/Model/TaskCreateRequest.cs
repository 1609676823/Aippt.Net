using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aippt.Net.Model
{
    /// <summary>
    /// 创建任务的请求体模型 (Request body model for creating a task)
    /// </summary>
    public class TaskCreateRequest
    {
        /// <summary>
        /// 创建任务的请求体模型 (Request body model for creating a task)
        /// </summary>
        public TaskCreateRequest() { }

        /// <summary>
        /// 创建任务的请求体模型 (Request body model for creating a task)
        /// </summary>
        /// <param name="type"></param>
        public TaskCreateRequest(int type)
        {
            this.type = type; // 注意这里赋值给的是 C# 属性名
        }

        /// <summary>
        /// 任务类型. 必传. (Task Type. Required.)
        /// 1: 智能生成 (Intelligent Generation)
        /// 3: 上传word (Upload Word)
        /// 4: 上传XMind (Upload XMind)
        /// 5: 上传FreeMind (Upload FreeMind)
        /// 6: 上传Markdown (Upload Markdown)
        /// 7: Markdown粘贴 (Markdown Paste)
        /// 8: 预置词 (Preset Words)
        /// 9: 上传PDF (Upload PDF)
        /// 10: 上传TXT (Upload TXT)
        /// 11: 自由输入 (Free Input)
        /// 12: 上传PPTX (Upload PPTX)
        /// 16: 导入URL链接 (Import URL Link)
        /// 17: 上传参考文档 (Upload Reference Document)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public int type { get; set; }

        /// <summary>
        /// 标题. 可选. (Title. Optional.)
        /// type=1、17: 用户输入标题 (必填 when type is 1 or 17) (User input title (required when type is 1 or 17))
        /// type=7: 粘贴Markdown的标题 (默认值为: Markdown粘贴文案) (Title for Markdown paste (defaults to: Markdown pasted text))
        /// type=11: 自由输入的标题 (默认值为: 自由输入) (Title for free input (defaults to: Free Input))
        /// type=3、4、5、6、8、9、10、12、16: 不传 (Not sent for these types)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public string? title { get; set; }

        /// <summary>
        /// 内容. 可选. (Content. Optional.)
        /// type=1、3、4、5、6、8、9、10、12、16、17: 不传 (Not sent for these types)
        /// type=7: 粘贴Markdown内容 (必填) (Pasted Markdown content (required))
        /// type=11: 自由输入内容 (必填) (后续调用上传Word生成(默认AI)) (Free input content (required) (subsequent calls use Upload Word generation (default AI)))
        /// </summary>
        // 移除 JsonPropertyName 特性
        public string? content { get; set; }

        /// <summary>
        /// 文件. 可选. (File. Optional.)
        /// Represents a single file's content or identifier depending on the upload mechanism.
        /// Depending on the context (e.NET Core), you might use IFormFile instead of byte[].
        /// 代表单个文件的内容或标识符，取决于上传机制。
        /// 根据具体上下文（例如 ASP.NET Core），你可能需要使用 IFormFile 而不是 byte[]。
        /// type=1、7、8、11、16、17: 不传 (Not sent for these types)
        /// type=3: word文件（必填）
        /// type=4: XMind文件（必填）
        /// type=5: FreeMind文件（必填）
        /// type=6: Markdown文件（必填）
        /// type=9: PDF文件(必填)
        /// type=10: TXT文件(必填)
        /// type=12: PPTX文件(必填)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public TaskFile? file { get; set; } // Using byte[]? as a generic representation

        /// <summary>
        /// 文件列表. 可选. (File List. Optional.)
        /// Represents multiple files' content or identifiers.
        /// 代表多个文件的内容或标识符。
        /// type=17: word文件、PDF文件、TXT文件(必填)(最多支持5个文件) (Word, PDF, TXT files (required) (supports up to 5 files))
        /// </summary>
        // 移除 JsonPropertyName 特性
        public List<TaskFile>? files { get; set; }  // Using List<byte[]>? as a generic representation

        /// <summary>
        /// 子类型 / 内容美化选项. 可选. (Sub Type / Content Beautification Option. Optional.)
        /// 1: 保持原文 (默认值) (Keep Original Text (default))
        /// 2: 适当扩写 (Appropriately Expand)
        /// 3: 润色美化 (Refine and Beautify)
        /// type=3、9、10、11 可传 (不传则取默认值) (Applicable for types 3, 9, 10, 11 (defaults if not sent))
        /// </summary>
        // 移除 JsonPropertyName 特性
        public int? sub_type { get; set; }

        /// <summary>
        /// 链接地址. 可选. (Link URL. Optional.)
        /// 上传的链接地址, type=16时必传 (The URL to upload from, required when type is 16)
        /// type=1、3、4、5、6、7、8、9、10、11、12、17: 不传 (Not sent for these types)
        /// type=16: URL链接 （必填）
        /// </summary>
        // 移除 JsonPropertyName 特性
        public string? link { get; set; }

        /// <summary>
        /// 模型. 可选. (Model. Optional.)
        /// glm4-air : 智谱模型 (Zhipu Model)
        /// deepSeek-v3：deepseek模型 (deepseek Model)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public string? model { get; set; }

        /// <summary>
        /// 是否开启联网搜索功能. 可选. (Enable Web Search Feature. Optional.)
        /// type=1,17时可传, 不传默认关闭 (Applicable for types 1, 17, defaults to disabled if not sent)
        /// 开启联网功能后，使用联网智能生成/上传参考文档接口获取内容结果 (When enabled, uses web search for content generation/reference document upload)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public bool? is_web_search { get; set; }

        /// <summary>
        /// 预置词ID. 可选. (Preset Word ID. Optional.)
        /// type=1、3、4、5、6、7、9、10、11、12、16、17: 不传 (Not sent for these types)
        /// type=8: 预置词列表接口返回的 id字段 （必填）
        /// </summary>
        // 移除 JsonPropertyName 特性
        public long? id { get; set; } // Assuming ID is likely a long or int based on "number"

        /// <summary>
        /// 高级选项 (JSON字符串). 可选. (Senior Options (JSON String). Optional.)
        /// 包含 page(页数), group(受众), scene(场景), tone(语气) (Includes page, group, scene, tone)
        /// 参考示意: {"page":3,"group":6,"scene":18,"tone":40} (Reference example)
        /// 具体参数请参考对应api接口返回数据 获取高级配置 (Refer to the corresponding API for specific parameters to get advanced configuration)
        /// </summary>
        // 移除 JsonPropertyName 特性
        public string? senior_options { get; set; }
    }
}
