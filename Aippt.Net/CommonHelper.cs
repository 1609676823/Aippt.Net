using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aippt.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string DecodeJson(string json)
        {
            string DecodeJson = string.Empty;
            try
            {
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    DecodeJson = JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                }
            }
            catch (Exception)
            {
                DecodeJson = json;
                //throw;
            }
            if (string.IsNullOrWhiteSpace(DecodeJson))
            {
                DecodeJson = json;
            }
            return DecodeJson;
        }

        /// <summary>
        /// Unix时间戳,定义为从格林威治时间1970年01月01日00时00分00秒起
        /// </summary>
        public static DateTimeOffset UnixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        /// <summary>
        ///  Unix 时间戳总毫秒数
        /// </summary>
        /// <returns></returns>
        public static long TimestampMillis()
        {
            //  return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            // 创建一个 DateTimeOffset 对象，表示 Unix 时间戳的起始时间（1970年1月1日）
            //// DateTimeOffset unixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

            // 计算当前时间与起始时间之间的时间间隔
            TimeSpan unixTimeSpan = DateTimeOffset.UtcNow - UnixStart;

            // 通过 TotalSeconds 属性将时间间隔转换为秒数，并将其转换为长整型
            long unixTimestamp = (long)unixTimeSpan.TotalMilliseconds;

            return unixTimestamp;
        }
        /// <summary>
        ///  Unix 时间戳总秒数
        /// </summary>
        /// <returns></returns>
        public static long TimestampSeconds()
        {
            // 计算当前时间与起始时间之间的时间间隔
            TimeSpan unixTimeSpan = DateTimeOffset.UtcNow - UnixStart;

            // 通过 TotalSeconds 属性将时间间隔转换为秒数，并将其转换为长整型
            long unixTimestamp = (long)unixTimeSpan.TotalSeconds;

            return unixTimestamp;

        }
        /// <summary>
        /// 判断地址最后是否为/，并补充
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetServerUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            try
            {
                if (!url.EndsWith("/"))
                {
                    return url + "/";
                }
            }
            catch
            {

                //throw;
            }

            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveDataPrefix(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            try
            {


                string trimmedInput = input.Trim();
                int index = trimmedInput.IndexOf("data:", StringComparison.OrdinalIgnoreCase);

                if (index == 0)
                {
                    return trimmedInput.Substring(5).Trim();
                }
            }
            catch (Exception)
            {

                //throw;
            }
            return input;
        }

        /// <summary>
        /// 移除某个前缀的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="removeKey"></param>
        /// <returns></returns>
        public static string RemovePrefixBykey(string input, string removeKey)
        {
            // 1. 基本的输入校验：如果输入字符串为空或null，直接返回空字符串
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // 2. 移除前缀键的校验：如果移除键为空或null，或者输入字符串短于移除键，
            //    则无法进行有效移除，直接返回原始输入（或者根据需求返回空字符串）
            if (string.IsNullOrWhiteSpace(removeKey) || input.Length < removeKey.Length)
            {
                return input; // 无法移除，返回原始输入
            }

            // 3. 移除两端空白字符，避免因空格导致匹配失败
            string trimmedInput = input.Trim();

            // 4. 检查输入字符串是否以指定的 `removeKey` 开头
            //    使用 OrdinalIgnoreCase 进行不区分大小写的比较，提高兼容性
            if (trimmedInput.StartsWith(removeKey, StringComparison.OrdinalIgnoreCase))
            {
                // 5. 如果是，则从 `removeKey` 之后的位置开始截取字符串
                //    并再次 Trim() 移除截取后可能出现的空白字符
                return trimmedInput.Substring(removeKey.Length).Trim();
            }
            else
            {
                // 6. 如果不以 `removeKey` 开头，则表示无法移除前缀，返回原始输入
                return input;
            }
        }
        /// <summary>
        /// 根据文件名或文件完整路径获取对应的 MIME 类型。
        /// Get the corresponding MIME type based on the file name or full file path.
        /// </summary>
        /// <param name="fileInput">文件名或文件完整路径，例如 "example.mp3"、"C:\files\example.mp3" 等。
        /// The file name or full file path, such as "example.mp3", "C:\files\example.mp3", etc.</param>
        /// <returns>返回对应的 MIME 类型，如果未找到匹配则返回 "application/octet-stream"。
        /// Returns the corresponding MIME type, or "application/octet-stream" if no match is found.</returns>


        // 可以使用 Dictionary 存储映射关系，这样更容易维护和扩展


        public static string GetMimeType(string fileInput)
        {
            if (string.IsNullOrEmpty(fileInput))
            {
                return "application/octet-stream"; // 输入无效，返回默认类型
            }

            // 提取文件扩展名
            string fileExtension = System.IO.Path.GetExtension(fileInput);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return "application/octet-stream"; // 没有扩展名，返回默认类型
            }

            // 移除点并转为小写，用于查找
            string extension = fileExtension.TrimStart('.').ToLower();

            // 从字典中查找 MIME 类型
            if (MimeTypeMappings.TryGetValue(extension, out string? mimeType))
            {
                return mimeType;
            }
            else
            {
                // 如果在映射中未找到匹配的扩展名，则返回通用的二进制数据 MIME 类型。
                return "application/octet-stream";
            }
        }

        private static readonly Dictionary<string, string> MimeTypeMappings = new Dictionary<string, string>()
        {
        // 文本类型 Text types
        { "txt", "text/plain" },
        { "csv", "text/csv" },
        { "html", "text/html" },
        { "htm", "text/html" },
        { "css", "text/css" },
        { "js", "application/javascript" },
        { "json", "application/json" }, // 常用，可以补充
        { "xml", "application/xml" },   // 常用，可以补充
        { "md", "text/markdown" },      // 补充 Markdown
        { "markdown", "text/markdown" },// 补充 Markdown

        // 图像类型 Image types
        { "jpg", "image/jpeg" },
        { "jpeg", "image/jpeg" },
        { "png", "image/png" },
        { "gif", "image/gif" },
        { "bmp", "image/bmp" },
        { "svg", "image/svg+xml" },
        { "webp", "image/webp" }, // 常用，可以补充

        // 音频类型 Audio types
        { "mp3", "audio/mpeg" },
        { "mpga", "audio/mpeg" },
        { "wav", "audio/wav" },
        { "m4a", "audio/mp4" },
        { "aac", "audio/aac" }, // 常用，可以补充

        // 视频类型 Video types
        { "mp4", "video/mp4" },
        { "mpeg", "video/mpeg" },
        { "webm", "video/webm" },
        { "avi", "video/x-msvideo" }, // 常用，可以补充
        { "mov", "video/quicktime" }, // 常用，可以补充

        // 文档类型 Document types
        { "pdf", "application/pdf" },
        { "doc", "application/msword" },
        { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { "xls", "application/vnd.ms-excel" },
        { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { "ppt", "application/vnd.ms-powerpoint" },
        { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },

        // 思维导图类型 Mind map types (非官方或常用约定)
        // XMind 文件实际上是 ZIP 压缩的 XML，也可以考虑 application/zip 或 application/xml
        { "xmind", "application/vnd.xmind.workbook" }, // 补充 XMind (一个常用表示法)
        { "mm", "application/x-freemind" },           // 补充 FreeMind (一个常用表示法)

        // 压缩文件类型 Compressed file types
        { "zip", "application/zip" },
        { "rar", "application/vnd.rar" },
        { "7z", "application/x-7z-compressed" }
        // 其他压缩类型如 .gz, .tar 等也可以按需补充
          };


    }
}
