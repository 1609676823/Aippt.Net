using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aippt.Net.Enum
{
    /// <summary>
    /// Represents the available sub-types for task creation, primarily related to content beautification options.
    /// These sub-types are applicable for specific TaskCreationType values such as UploadWord, UploadPdf, UploadTxt, and FreeInput.
    /// 表示任务创建的可用子类型，主要与内容美化选项相关。
    /// 这些子类型适用于特定的 TaskCreationType 值，例如 UploadWord、UploadPdf、UploadTxt 和 FreeInput。
    /// </summary>
    public static class TaskCreationSubType
    {
        /// <summary>
        /// 保持原文 (默认值)
        /// Keep original text (default value).
        /// This is the default value if no sub_type is provided for applicable task creation types.
        /// Applicable when TaskCreationType is UploadWord ("3"), UploadPdf ("9"), UploadTxt ("10"), or FreeInput ("11").
        /// </summary>
        public const string KeepOriginal = "1";

        /// <summary>
        /// 适当扩写
        /// Appropriately expand.
        /// Applicable when TaskCreationType is UploadWord ("3"), UploadPdf ("9"), UploadTxt ("10"), or FreeInput ("11").
        /// </summary>
        public const string AppropriatelyExpand = "2";

        /// <summary>
        /// 润色美化
        /// Refine and beautify.
        /// Applicable when TaskCreationType is UploadWord ("3"), UploadPdf ("9"), UploadTxt ("10"), or FreeInput ("11").
        /// </summary>
        public const string RefineAndBeautify = "3";

        // Note: This sub_type is typically used when TaskCreationType is "3", "9", "10", or "11".
        // If not provided for these types, the default value "1" (KeepOriginal) is used.
    }
}
