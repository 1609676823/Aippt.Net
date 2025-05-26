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
    /// 用户任务响应类，包含返回码、数据和提示信息
    /// User task response class, containing return code, data, and prompt information
    /// </summary>
    public class UserTaskResponse
    {
        /// <summary>
        /// 返回码，0表示成功，非0表示异常
        /// Return code, 0 indicates success, non-zero indicates an exception
        /// </summary>
        public long code { get; set; }

        /// <summary>
        /// 包含用户任务相关数据的对象列表
        /// List of objects containing user task-related data
        /// </summary>
        public List<UserTaskData>? data { get; set; } = new List<UserTaskData>();

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
                    DeserializeUserTaskResponse(value!);
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
        /// Deserialize a JSON string into the current instance.
        /// This method parses each property in the JSON and assigns the values to the corresponding properties of the current instance.
        /// If an exception occurs during the parsing process, it catches the exception and continues to parse the subsequent properties.
        /// </summary>
        /// <param name="json">需要解析的 JSON 字符串。The JSON string to be parsed.</param>
        public virtual void DeserializeUserTaskResponse(string json)
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
                catch { /* Handle or log exception if needed */ }

                try
                {
                    var msgNode = jsonNode["msg"];
                    if (msgNode != null)
                    {
                        this.msg = msgNode.GetValue<string>();
                    }
                }
                catch { /* Handle or log exception if needed */ }

                // Now 'data' is directly a JSON array
                var dataArrayNode = jsonNode["data"]?.AsArray();
                if (dataArrayNode != null)
                {
                    this.data = new List<UserTaskData>(); // Initialize the list
                    foreach (var itemNode in dataArrayNode)
                    {
                        if (itemNode != null)
                        {
                            var userTask = new UserTaskData();
                            try
                            {
                                var uidNode = itemNode["uid"];
                                if (uidNode != null)
                                {
                                    userTask.uid = uidNode.GetValue<string>();
                                }
                            }
                            catch { /* Handle or log exception for uid */ }

                            try
                            {
                                var countNode = itemNode["count"];
                                if (countNode != null)
                                {
                                    userTask.count = countNode.GetValue<long>();
                                }
                            }
                            catch { /* Handle or log exception for count */ }
                            this.data.Add(userTask);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 用户任务数据类，包含用户ID和任务计数信息
    /// User task data class, containing user ID and task count information
    /// </summary>
    public class UserTaskData
    {
        /// <summary>
        /// 用户唯一标识
        /// Unique identifier of the user
        /// </summary>
        public string? uid { get; set; }

        /// <summary>
        /// 任务数量
        /// Number of tasks
        /// </summary>
        public long count { get; set; }
    }
}
