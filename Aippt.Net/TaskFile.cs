using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aippt.Net
{
    /// <summary>
    /// 任务文件构造类
    /// </summary>
    public class TaskFile
    {
        /// <summary>
        /// 任务文件构造类
        /// </summary>
        public TaskFile() { }
        /// <summary>
        /// 任务文件构造类
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public TaskFile(string fileName, byte[]? fileByte=null)
        {
            FileName = fileName;
            if (fileByte != null) { FileByte = fileByte; }
            
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName {  get; set; }=string.Empty;

        /// <summary>
        /// 文件的二进制流
        /// </summary>
        public byte[]? FileByte { get; set; }

        private string _filePath=string.Empty;
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value!;
                try
                {
                    FileByte= FileToBinaryConverter.ConvertFileToBinary(value!);
                    //DeserializeTokenResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
                }
            }
        }

        private string  _base64String=string.Empty;
        /// <summary>
        /// base64字符串
        /// </summary>
        public string? Base64String
        {
            get { return _filePath; }
            set
            {
                _base64String = value!;
                try
                {
                    FileByte = FileToBinaryConverter.ConvertBase64ToBinary(value!);
                    //DeserializeTokenResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
                }
            }
        }


        private string _url = string.Empty;

        /// <summary>
        /// url地址
        /// </summary>
        public string? Url
        {
            get { return _url; }
            set
            {
                _url = value!;
                try
                {
                    FileByte = FileToBinaryConverter.ConvertUrlToBinary(value!);
                    //DeserializeTokenResponse(value!);
                }
                catch (Exception)
                {
                    // 可添加日志记录等操作
                }
            }
        }

    }
}
