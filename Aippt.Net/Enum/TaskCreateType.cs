using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aippt.Net.Enum
{
    /// <summary>
    /// Represents the available types for task creation.
    /// 表示任务创建的可用类型。
    /// </summary>
    public static class TaskCreateType
    {
        /// <summary>
        /// 智能生成 (查看结果: "智能生成")
        /// Smart Generate (View result: "智能生成").
        /// </summary>
        public const int SmartGenerate = 1;

        /// <summary>
        /// 上传word (查看结果: "上传Word")
        /// Upload Word (View result: "上传Word").
        /// </summary>
        public const int UploadWord = 3;

        /// <summary>
        /// 上传XMind (查看结果: "上传普通文件")
        /// Upload XMind (View result: "上传普通文件").
        /// </summary>
        public const int UploadXMind = 4;

        /// <summary>
        /// 上传FreeMind (查看结果: "上传普通文件")
        /// Upload FreeMind (View result: "上传普通文件").
        /// </summary>
        public const int UploadFreeMind = 5;

        /// <summary>
        /// 上传Markdown (查看结果: "上传普通文件")
        /// Upload Markdown (View result: "上传普通文件").
        /// </summary>
        public const int UploadMarkdown = 6;

        /// <summary>
        /// Markdown粘贴 (创建任务后可直接调用作品保存)
        /// Markdown Paste (Can directly call save after task creation).
        /// </summary>
        public const int MarkdownPaste = 7;

        /// <summary>
        /// 预置词 (创建任务后可直接调用作品保存)
        /// Preset Words (Can directly call save after task creation).
        /// </summary>
        public const int PresetWords = 8;

        /// <summary>
        /// 上传PDF (查看结果: "上传Word")
        /// Upload PDF (View result: "上传Word").
        /// </summary>
        public const int UploadPdf = 9;

        /// <summary>
        /// 上传TXT (查看结果: "上传Word")
        /// Upload TXT (View result: "上传Word").
        /// </summary>
        public const int UploadTxt = 10;

        /// <summary>
        /// 自由输入 (查看结果: "上传Word")
        /// Free Input (View result: "上传Word").
        /// </summary>
        public const int FreeInput = 11;

        /// <summary>
        /// 上传PPTX (查看结果: "上传Word")
        /// Upload PPTX (View result: "上传Word").
        /// </summary>
        public const int UploadPptx = 12;

        /// <summary>
        /// 导入URL链接 (查看结果: "导入URL链接")
        /// Import URL Link (View result: "导入URL链接").
        /// </summary>
        public const int ImportUrlLink = 16;

        /// <summary>
        /// 上传参考文档 (查看结果: "上传参考文档")
        /// Upload Reference Document (View result: "上传参考文档").
        /// </summary>
        public const int UploadReferenceDocument = 17;
    }
}
