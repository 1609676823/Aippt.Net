using Aippt.Net.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aippt.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class AipptClient : AipptClientBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AipptClient() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="secretkey"></param>
        /// <param name="userid"></param>
        /// <param name="apiurl"></param>
        /// <param name="channel"></param>
        public AipptClient(string apikey, string secretkey, string userid, string apiurl = "https://co.aippt.cn/", string channel = "") : base(apikey, secretkey, userid, apiurl, channel) { }

        #region 1.获取高级配置
        /// <summary>
        /// 获取高级配置 ASYNC
        /// </summary>
        /// <returns></returns>
        public async Task<SeniorOptionResponse> GetSeniorOptionAsync()
        {

            string url = Url + "api/ai/chat/senior/option";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url);


            SeniorOptionResponse seniorOptionResponse = new SeniorOptionResponse();
            seniorOptionResponse.RealJsonstring = resjosn;

            return seniorOptionResponse;
        }

        /// <summary>
        /// 获取高级配置
        /// </summary>
        /// <returns></returns>
        public SeniorOptionResponse GetSeniorOption()
        {
            Task<SeniorOptionResponse> task = GetSeniorOptionAsync();
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 2.任务创建
        /// <summary>
        /// 生成PPT须先调用此接口创建任务, 获取任务ID后方可进行后续生成操作
        /// </summary>
        /// <param name="taskCreateRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskCreateResponse> TaskCreateAsync(TaskCreateRequest taskCreateRequest)
        {
            if (taskCreateRequest == null)
            {
                throw new ArgumentNullException("TaskCreateRequest is not null");
            }

            // 假设你已经有一个 TaskCreateRequest 的实例 taskCreateRequest
            // 例如:
            // TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
            // taskCreateRequest.type = 1;
            // taskCreateRequest.title = "智能生成标题";
            // ... 设置其他属性

            using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

            // 1. 添加 type (必传属性)
            // type 是 int 类型，非空，所以直接添加
            multipartFormDataContent.Add(new StringContent(taskCreateRequest.type.ToString()), "type");

            // 2. 添加 title (可选，但在特定type下必填，这里只判断是否为空)
            // 即使在 type=1 或 17 时是必填，从构建 MultipartFormData 的角度，如果属性值为 null 或空字符串，我们也无法添加到 FormData 中
            if (!string.IsNullOrWhiteSpace(taskCreateRequest.title))
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.title), "title");
            }

            // 3. 添加 content (可选，但在特定type下必填，这里只判断是否为空)
            if (!string.IsNullOrWhiteSpace(taskCreateRequest.content))
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.content), "content");
            }

            // 4. 添加 file (可选，单文件上传时必填，这里只判断是否为 null)
            // file 是 byte[] 类型，表示文件内容
            if (taskCreateRequest.file != null && taskCreateRequest.file.FileByte!.Length > 0)
            {
                // 对于文件内容，使用 ByteArrayContent
                // 注意：根据实际API需求，可能还需要提供文件名。如果API不强制要求特定文件名，可以省略第三个参数，
                // 或者使用一个默认的名称，例如 "file.bin" 或根据原始文件名推断。
                // 这里假设 API 只需要文件内容和字段名 "file"
                ByteArrayContent fileContent = new ByteArrayContent(taskCreateRequest.file.FileByte!);
                //fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(CommonHelper.GetMimeType(taskCreateRequest.file.FileName));
                multipartFormDataContent.Add(fileContent, "file", taskCreateRequest.file.FileName);
                // multipartFormDataContent.Add(new ByteArrayContent(taskCreateRequest.file), "file");

                // 如果需要提供文件名，例如 file.txt:
                // multipartFormDataContent.Add(new ByteArrayContent(taskCreateRequest.file), "file", "your_filename.ext");
            }

            // 5. 添加 files (可选，多文件上传时必填，这里只判断列表是否为 null 或空)
            // files 是 List<byte[]> 类型，表示多个文件内容
            if (taskCreateRequest.files != null && taskCreateRequest.files.Count > 0)
            {
                // 对于文件列表，通常需要为列表中的每个文件添加一个 Multipart section，
                // 字段名通常是列表名，或者带有索引的列表名（取决于API设计）
                // 这里假设 API 期望多个字段名为 "files" 的文件内容
                int fileIndex = 0;
                foreach (var item in taskCreateRequest.files)
                {
                    if (item != null && item.FileByte!.Length > 0)
                    {

                        ByteArrayContent fileContent = new ByteArrayContent(item.FileByte);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(CommonHelper.GetMimeType(item.FileName));
                        multipartFormDataContent.Add(fileContent, "files", item.FileName);

                        // 根据API，可能使用相同的字段名 "files"
                        // multipartFormDataContent.Add(new ByteArrayContent(item.FileByte),"files", item.FileName); // 使用索引和默认文件名，或者根据需要调整文件名
                        fileIndex++;

                        // 或者如果 API 期望字段名带索引，例如 files[0], files[1]
                        // multipartFormDataContent.Add(new ByteArrayContent(fileBytes), $"files[{fileIndex}]", $"file_{fileIndex}.bin");
                        // fileIndex++;
                    }
                }
            }


            // 6. 添加 sub_type (可选)
            // sub_type 是 int? 类型，判断 HasValue
            if (taskCreateRequest.sub_type.HasValue)
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.sub_type.Value.ToString()), "sub_type");
            }

            // 7. 添加 link (可选，但在 type=16 时必填，这里只判断是否为空)
            if (!string.IsNullOrWhiteSpace(taskCreateRequest.link))
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.link), "link");
            }

            // 8. 添加 model (可选)
            if (!string.IsNullOrWhiteSpace(taskCreateRequest.model))
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.model), "model");
            }

            // 9. 添加 is_web_search (可选)
            // is_web_search 是 bool? 类型，判断 HasValue
            if (taskCreateRequest.is_web_search.HasValue)
            {
                // 布尔值通常转换为 "true" 或 "false" 的字符串
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.is_web_search.Value.ToString().ToLower()), "is_web_search");
            }

            // 10. 添加 id (可选，但在 type=8 时必填，这里只判断 HasValue)
            // id 是 long? 类型，判断 HasValue
            if (taskCreateRequest.id.HasValue)
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.id.Value.ToString()), "id");
            }

            // 11. 添加 senior_options (可选)
            // senior_options 是 string? 类型，判断是否为空
            if (!string.IsNullOrWhiteSpace(taskCreateRequest.senior_options))
            {
                multipartFormDataContent.Add(new StringContent(taskCreateRequest.senior_options), "senior_options");
            }

            // 至此，multipartFormDataContent 对象已经根据 taskCreateRequest 的属性值填充完毕
            // 接下来你可以使用这个对象进行 HTTP 请求，例如:
            // using (HttpClient client = new HttpClient())
            // {
            //     HttpResponseMessage response = await client.PostAsync("your_api_endpoint", multipartFormDataContent);
            //     // 处理响应
            // }

            // 注意：使用完 multipartFormDataContent 后，记得 Dispose
            // multipartFormDataContent.Dispose();

            string url = Url + "api/ai/chat/v2/task";
            string resjosn = await SendRequestByFormDataAsync(url: url, multipartFormData: multipartFormDataContent);
            try
            {
                multipartFormDataContent.Dispose();
            }
            catch (Exception)
            {

                // throw;
            }

            TaskCreateResponse taskCreateResponse = new TaskCreateResponse();
            taskCreateResponse.RealJsonstring = resjosn;

            return taskCreateResponse;
        }

        /// <summary>
        /// 生成PPT须先调用此接口创建任务, 获取任务ID后方可进行后续生成操作
        /// </summary>
        /// <param name="taskCreateRequest"></param>
        /// <returns></returns>
        public TaskCreateResponse TaskCreate(TaskCreateRequest taskCreateRequest)
        {

            Task<TaskCreateResponse> task = TaskCreateAsync(taskCreateRequest);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 3.智能生成-默认AI生成-步骤1.标题生成大纲


        /// <summary>
        /// 智能生成-默认AI生成-标题生成大纲
        /// 创建type=1（智能生成）任务并获取任务ID后，调用此接口生成ppt大纲
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateOutlineAsync(string task_id)
        {
            string url = Url + "api/ai/chat/outline";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            //SeniorOptionResponse seniorOptionResponse = new SeniorOptionResponse();
            //seniorOptionResponse.RealJsonstring = resjosn;
            return resjosn;
        }
        /// <summary>
        /// 智能生成-默认AI生成-标题生成大纲
        /// 创建type=1（智能生成）任务并获取任务ID后，调用此接口生成ppt大纲
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateOutline(string task_id)
        {
            Task<string> task = CreateOutlineAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 4.智能生成-默认AI生成-步骤2.大纲生成内容 (新)

        /// <summary>
        /// 大纲生成内容 (新) ASYNC
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<ContentResponse> CreateContentAsync(string task_id)
        {

            string url = Url + "api/ai/chat/v2/content";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);

            ContentResponse contentResponse = new ContentResponse();
            contentResponse.RealJsonstring = resjosn;

            return contentResponse;
        }

        /// <summary>
        /// 大纲生成内容 (新)
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public ContentResponse CreateContent(string task_id)
        {
            Task<ContentResponse> task = CreateContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 5.智能生成-默认AI生成-步骤3.大纲生成内容结果查询 (新)
        /// <summary>
        /// 智能生成-默认AI生成-步骤3.大纲生成内容结果查询 (新)
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public async Task<CheckContentResponse> CheckContentAsync(string ticket)
        {

            string url = Url + "api/ai/chat/v2/content/check";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("ticket", ticket);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            CheckContentResponse checkContentResponse = new CheckContentResponse();
            checkContentResponse.RealJsonstring = resjosn;
            return checkContentResponse;
        }
        /// <summary>
        /// 智能生成-默认AI生成-步骤3.大纲生成内容结果查询 (新)
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public CheckContentResponse CheckContent(string ticket)
        {
            Task<CheckContentResponse> task = CheckContentAsync(ticket);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 6.智能生成-百度AI生成-步骤1.标题生成大纲
        /// <summary>
        /// 智能生成-百度AI生成-步骤1.标题生成大纲
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateWxOutlineAsync(string task_id)
        {
            string url = Url + "api/ai/chat/wx/outline";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            return resjosn;
        }
        /// <summary>
        /// 智能生成-百度AI生成-步骤1.标题生成大纲
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateWxOutline(string task_id)
        {
            Task<string> task = CreateWxOutlineAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 7.智能生成-百度AI生成-步骤2.大纲生成内容
        /// <summary>
        /// 智能生成-百度AI生成-步骤2.大纲生成内容 ASYNC
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<ContentResponse> CreateWxContentAsync(string task_id)
        {

            string url = Url + "api/ai/chat/wx/content";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);

            ContentResponse contentResponse = new ContentResponse();
            contentResponse.RealJsonstring = resjosn;

            return contentResponse;
        }

        /// <summary>
        /// 智能生成-百度AI生成-步骤2.大纲生成内容
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public ContentResponse CreateWxContent(string task_id)
        {
            Task<ContentResponse> task = CreateWxContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 8.上传Word-导入Word生成内容获取
        /// <summary>
        /// 上传Word-导入Word生成内容获取ASYNC
        /// 用户上传word，前端解析 word 获取内容后请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateWordOutlineContentAsync(string task_id)
        {
            string url = Url + "api/ai/chat/v2/word";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            return resjosn;
        }
        /// <summary>
        /// 上传Word-导入Word生成内容获取
        /// 用户上传word，前端解析 word 获取内容后请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateWordOutlineContent(string task_id)
        {
            Task<string> task = CreateWordOutlineContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 9.上传参考文档-参考文档导入生成内容获取
        /// <summary>
        /// 上传参考文档-参考文档导入生成内容获取 ASYNC
        /// 用户通过参考文档输入标题和上传word、pdf、txt进行创建任务实现智能生成内容
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateReferOutlineContentAsync(string task_id)
        {
            string url = Url + "api/ai/chat/refer";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            return resjosn;
        }
        /// <summary>
        /// 上传参考文档-参考文档导入生成内容获取 
        /// 用户通过参考文档输入标题和上传word、pdf、txt进行创建任务实现智能生成内容
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateReferOutlineContent(string task_id)
        {
            Task<string> task = CreateReferOutlineContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 10.上传普通文件-Xmind、Freemind、Markdown文件结果获取
        /// <summary>
        /// 上传普通文件-Xmind、Freemind、Markdown文件结果获取 ASYNC
        /// Xmind、Freemind、Markdown文件结果获取
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="type">4.XMind导入;5.FreeMind导入;6.Markdown导入</param>
        /// <returns></returns>
        public async Task<MarkdownParseResponse> CreateConverFileMarkdownAsync(string task_id, string type)
        {
            string url = Url + "api/ai/conver/file";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            QuerykeyValuePairs.Add("type", type);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            MarkdownParseResponse markdownParseResponse = new MarkdownParseResponse();
            markdownParseResponse.RealJsonstring = resjosn;
            return markdownParseResponse;
        }
        /// <summary>
        /// 上传普通文件-Xmind、Freemind、Markdown文件结果获取
        /// Xmind、Freemind、Markdown文件结果获取
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="type">4.XMind导入;5.FreeMind导入;6.Markdown导入</param>
        /// <returns></returns>
        public MarkdownParseResponse CreateConverFileMarkdown(string task_id, string type)
        {
            Task<MarkdownParseResponse> task = CreateConverFileMarkdownAsync(task_id, type);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 11.联网智能生成-联网智能生成内容获取
        /// <summary>
        /// 联网智能生成-联网智能生成内容获取 ASYNC
        /// 用户创建type=1且开启联网功能的智能生成任务后，请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateNetworkOutlineContentAsync(string task_id)
        {
            string url = Url + "api/ai/chat/network";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            return resjosn;
        }
        /// <summary>
        /// 联网智能生成-联网智能生成内容获取
        /// 用户创建type=1且开启联网功能的智能生成任务后，请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateNetworkOutlineContent(string task_id)
        {
            Task<string> task = CreateNetworkOutlineContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 12.导入URL链接-导入链接生成内容获取
        /// <summary>
        /// 导入URL链接-导入链接生成内容获取 ASYNC
        /// 用户导入URL链接，获取网站内容后请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<string> CreateLinkOutlineContentAsync(string task_id)
        {
            string url = Url + "api/ai/chat/link";
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("task_id", task_id);
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs, streamResponse: true);
            return resjosn;
        }
        /// <summary>
        /// 导入URL链接-导入链接生成内容获取 
        /// 用户导入URL链接，获取网站内容后请求该接口生成大纲+内容的结果
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public string CreateLinkOutlineContent(string task_id)
        {
            Task<string> task = CreateLinkOutlineContentAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 13.获取PPT数据结构-任务ID获取PPT树形结构
        /// <summary>
        /// 获取PPT数据结构-任务ID获取PPT树形结构 ASYNC
        /// 此接口是获取任务的大纲内容对应的PPT树形结构。
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<PptTreeResponse> GetPptTreeByTaskidAsync(string task_id)
        {
            string url = Url + "api/generate/data";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("task_id", task_id));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            PptTreeResponse pptTreeResponse = new PptTreeResponse();
            pptTreeResponse.RealJsonstring = resjosn;
            return pptTreeResponse;
        }
        /// <summary>
        /// 获取PPT数据结构-任务ID获取PPT树形结构 
        /// 此接口是获取任务的大纲内容对应的PPT树形结构。
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public PptTreeResponse GetPptTreeByTaskid(string task_id)
        {
            Task<PptTreeResponse> task = GetPptTreeByTaskidAsync(task_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 14.作品ID获取PPT树形结构
        /// <summary>
        /// 作品ID获取PPT树形结构 ASYNC
        /// 此接口是获取任务的大纲内容对应的PPT树形结构。
        /// </summary>
        /// <param name="design_id"></param>
        /// <returns></returns>
        public async Task<PptTreeResponse> GetPptTreeByDesignidAsync(string design_id)
        {
            string url = Url + "api/generate/ppt-tree";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("design_id", design_id));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            PptTreeResponse pptTreeResponse = new PptTreeResponse();
            pptTreeResponse.RealJsonstring = resjosn;
            return pptTreeResponse;
        }
        /// <summary>
        /// 作品ID获取PPT树形结构 
        /// 此接口是获取任务的大纲内容对应的PPT树形结构。
        /// </summary>
        /// <param name="design_id"></param>
        /// <returns></returns>
        public PptTreeResponse GetPptTreeByDesignid(string design_id)
        {
            Task<PptTreeResponse> task = GetPptTreeByDesignidAsync(design_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 15.导出大纲、完整内容
        /// <summary>
        /// 此接口用于导出大纲或完整内容。ASYNC
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="type">1=导出markdown文件，2=导出思维导图图片(格式png)</param>
        /// <returns></returns>
        public async Task<GenerateFileResponse> GetGenerateFileAsync(string task_id, string type)
        {
            string url = Url + "api/generate/file";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("task_id", task_id));
            UrlEncoded.Add(new KeyValuePair<string, string>("type", type));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            GenerateFileResponse generateFileResponse = new GenerateFileResponse();
            generateFileResponse.RealJsonstring = resjosn;
            return generateFileResponse;
        }
        /// <summary>
        /// 此接口用于导出大纲或完整内容。
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="type">1=导出markdown文件，2=导出思维导图图片(格式png)</param>
        /// <returns></returns>
        public GenerateFileResponse GetGenerateFile(string task_id, string type)
        {
            Task<GenerateFileResponse> task = GetGenerateFileAsync(task_id, type);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 16.获取用户大纲次数

        /// <summary>
        /// 获取用户大纲次数 ASYNC
        /// </summary>
        /// <param name="uids">企业方自己用户的唯一标识列表，数量最多为20个</param>
        /// <param name="channel">渠道</param>
        /// <returns></returns>
        public async Task<UserTaskResponse> GetUserTaskAsync(List<string> uids, string channel = "")
        {
            string url = Url + "api/statistic/user/task";
            long timestamp = CommonHelper.TimestampSeconds();
            string data = "POST@/api/statistic/user/task/@" + timestamp;
            string signature = GenHmac(data, this.SecretKey);
            Dictionary<string, string> headerdic = new Dictionary<string, string>();
            headerdic.Add("x-api-key", base.ApiKey);
            headerdic.Add("x-timestamp", timestamp.ToString());
            headerdic.Add("x-signature", signature);
            headerdic.Add("Accept", "*/*");
            headerdic.Add("Connection", "keep-alive");
            // headerdic.Add("Content-Type", "application/x-www-form-urlencoded");
            //foreach (var item in headerdic)
            //{
            //    Console.WriteLine(item.Key + ":" + item.Value);
            //}

            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            foreach (string uid in uids)
            {
                UrlEncoded.Add(new KeyValuePair<string, string>("uid", uid));
            }
            UrlEncoded.Add(new KeyValuePair<string, string>("channel", channel));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, requestheaders: headerdic, body_UrlEncoded: UrlEncoded, UseAuthHeadersToken: false);
            UserTaskResponse userTaskResponse = new UserTaskResponse();
            userTaskResponse.RealJsonstring = resjosn;
            return userTaskResponse;
        }

        /// <summary>
        /// 获取用户大纲次数
        /// </summary>
        /// <param name="uids">企业方自己用户的唯一标识列表，数量最多为20个</param>
        /// <param name="channel">渠道</param>
        /// <returns></returns>
        public UserTaskResponse GetUserTask(List<string> uids, string channel = "")
        {
            Task<UserTaskResponse> task = GetUserTaskAsync(uids, channel);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 17.编辑大纲
        /// <summary>
        /// API编辑大纲接口 ASYNC, 参数 content 需要根据任务类型去传对应的值:
        ///1. 智能生成(type = 1), 需要传修改过的完整大纲数据
        ///2. 其他类型, 需要传修改过的完整大纲+内容数据
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="content">该值为 解析Markdown文本生成PPT树形结构接口 返回数据data下内容的修改后的json字符串
        ///任务类型type=1: 修改过的完整大纲数据
        ///任务类型为其他: 修改过的完整大纲+内容数据</param>
        /// <returns></returns>
        public async Task<OutlineSaveResponse> OutlineSaveAsync(string task_id, string content)
        {
            string url = Url + "api/ai/chat/v2/outline/save";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("task_id", task_id));
            UrlEncoded.Add(new KeyValuePair<string, string>("content", content));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            OutlineSaveResponse outlineSaveResponse = new OutlineSaveResponse();
            outlineSaveResponse.RealJsonstring = resjosn;
            return outlineSaveResponse;
        }
        /// <summary>
        /// API编辑大纲接口, 参数 content 需要根据任务类型去传对应的值:
        ///1. 智能生成(type = 1), 需要传修改过的完整大纲数据
        ///2. 其他类型, 需要传修改过的完整大纲+内容数据
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="content">该值为 解析Markdown文本生成PPT树形结构接口 返回数据data下内容的修改后的json字符串
        ///任务类型type=1: 修改过的完整大纲数据
        ///任务类型为其他: 修改过的完整大纲+内容数据</param>
        /// <returns></returns>
        public OutlineSaveResponse OutlineSave(string task_id, string content)
        {
            Task<OutlineSaveResponse> task = OutlineSaveAsync(task_id, content);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 18.模板套装列表筛选项
        /// <summary>
        /// 此接口返回模板套装列表筛选项，用于模板套装列表接口的筛选项数据填充 ASYNC
        /// </summary>
        /// <returns></returns>
        public async Task<SuitSelectResponse> GetSuitSelectAsync()
        {

            string url = Url + "api/template_component/suit/select";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url);
            SuitSelectResponse suitSelectResponse = new SuitSelectResponse();
            suitSelectResponse.RealJsonstring = resjosn;
            return suitSelectResponse;
        }
        /// <summary>
        /// 此接口返回模板套装列表筛选项，用于模板套装列表接口的筛选项数据填充
        /// </summary>
        /// <returns></returns>
        public SuitSelectResponse GetSuitSelect()
        {
            Task<SuitSelectResponse> task = GetSuitSelectAsync();
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 19.模板套装列表

        /// <summary>
        /// 此接口返回模板套装列表，选中后的套装用于调用 PPT生成 接口 ASYNC。
        /// </summary>
        /// <param name="suitSearchRequest"></param>
        /// <returns></returns>
        public async Task<SuitSearchResponse> SuitSearchAsync(SuitSearchRequest? suitSearchRequest = null)
        {

            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            if (suitSearchRequest != null)
            {


                if (suitSearchRequest.scene_id != null)
                {
                    QuerykeyValuePairs.Add("scene_id", suitSearchRequest.scene_id.ToString()!);
                }

                if (suitSearchRequest.style_id != null)
                {
                    QuerykeyValuePairs.Add("style_id", suitSearchRequest.style_id.ToString()!);
                }

                if (suitSearchRequest.colour_id != null)
                {
                    QuerykeyValuePairs.Add("colour_id", suitSearchRequest.colour_id.ToString()!);
                }

                if (suitSearchRequest.page != null)
                {
                    QuerykeyValuePairs.Add("page", suitSearchRequest.page.ToString()!);
                }

                if (suitSearchRequest.page_size != null)
                {
                    QuerykeyValuePairs.Add("page_size", suitSearchRequest.page_size.ToString()!);
                }

            }
            string url = Url + "api/template_component/suit/search";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            SuitSearchResponse suitSearchResponse = new SuitSearchResponse();
            suitSearchResponse.RealJsonstring = resjosn;

            return suitSearchResponse;
        }

        /// <summary>
        /// 此接口返回模板套装列表，选中后的套装用于调用 PPT生成 接口 。
        /// </summary>
        /// <param name="suitSearchReques"></param>
        /// <returns></returns>
        public SuitSearchResponse SuitSearch(SuitSearchRequest? suitSearchReques)
        {
            Task<SuitSearchResponse> task = SuitSearchAsync(suitSearchReques);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 20.企业模板套装列表
        /// <summary>
        /// 此接口返回企业模板套装列表，选中后的套装用于调用 PPT生成 接口。
        /// </summary>

        /// <returns></returns>
        public async Task<SuitSearchResponse> EnterpriseSuitSearchAsync()
        {

            //Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            //if (suitSearchRequest != null)
            //{


            //    if (suitSearchRequest.scene_id != null)
            //    {
            //        QuerykeyValuePairs.Add("scene_id", suitSearchRequest.scene_id.ToString()!);
            //    }

            //    if (suitSearchRequest.style_id != null)
            //    {
            //        QuerykeyValuePairs.Add("style_id", suitSearchRequest.style_id.ToString()!);
            //    }

            //    if (suitSearchRequest.colour_id != null)
            //    {
            //        QuerykeyValuePairs.Add("colour_id", suitSearchRequest.colour_id.ToString()!);
            //    }

            //    if (suitSearchRequest.page != null)
            //    {
            //        QuerykeyValuePairs.Add("page", suitSearchRequest.page.ToString()!);
            //    }

            //    if (suitSearchRequest.page_size != null)
            //    {
            //        QuerykeyValuePairs.Add("page_size", suitSearchRequest.page_size.ToString()!);
            //    }

            //}
            string url = Url + "api/template_component/enterprise/suit/list";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url);
            SuitSearchResponse suitSearchResponse = new SuitSearchResponse();
            suitSearchResponse.RealJsonstring = resjosn;

            return suitSearchResponse;
        }

        /// <summary>
        /// 此接口返回企业模板套装列表，选中后的套装用于调用 PPT生成 接口。
        /// </summary>

        /// <returns></returns>
        public SuitSearchResponse EnterpriseSuitSearch()
        {
            Task<SuitSearchResponse> task = EnterpriseSuitSearchAsync();
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 21.作品生成
        /// <summary>
        /// 此接口通过任务id，与用户选择或动态匹配的模版数据的模版id生成PPT画布数据并保存作品。 ASYNC
        /// </summary>
        /// <param name="task_id">任务ID 必传</param>
        /// <param name="template_id">模版套装id 必传</param>
        /// <param name="name">作品名称 （不传默认: 大纲标题） 非必传</param>
        /// <param name="template_type">默认为1，1：系统模板，2: 企业模板  非必传</param>
        /// <returns></returns>
        public async Task<DesignSaveResponse> DesignSaveAsync(string task_id, string template_id, string? name = "", string template_type = "")
        {
            string url = Url + "api/design/v2/save";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("task_id", task_id));
            UrlEncoded.Add(new KeyValuePair<string, string>("template_id", template_id));
            if (!string.IsNullOrWhiteSpace(name)) { UrlEncoded.Add(new KeyValuePair<string, string>("name", name!)); }
            if (!string.IsNullOrWhiteSpace(template_type)) { UrlEncoded.Add(new KeyValuePair<string, string>("template_type", template_type!)); }
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            DesignSaveResponse designSaveResponse = new DesignSaveResponse();
            designSaveResponse.RealJsonstring = resjosn;
            return designSaveResponse;
        }
        /// <summary>
        /// 此接口通过任务id，与用户选择或动态匹配的模版数据的模版id生成PPT画布数据并保存作品。
        /// </summary>
        /// <param name="task_id">任务ID 必传</param>
        /// <param name="template_id">模版套装id 必传</param>
        /// <param name="name">作品名称 （不传默认: 大纲标题） 非必传</param>
        /// <param name="template_type">默认为1，1：系统模板，2: 企业模板  非必传</param>
        /// <returns></returns>
        public DesignSaveResponse DesignSave(string task_id, string template_id, string? name = "", string template_type = "")
        {
            Task<DesignSaveResponse> task = DesignSaveAsync(task_id, template_id, name, template_type);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 22.作品列表
        /// <summary>
        /// 此接口返回第三方用户作品列表 ASYNC
        /// </summary>
        /// <param name="order">排序 1最新创建 2最近修改</param>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public async Task<DesignListResponse> DesignListAsync(string order, string? page = "", string? page_size = "")
        {
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("order", order);
            if (!string.IsNullOrWhiteSpace(page))
            {
                QuerykeyValuePairs.Add("page", page!);
            }
            if (!string.IsNullOrWhiteSpace(page_size))
            {
                QuerykeyValuePairs.Add("page_size", page_size!);
            }

            string url = Url + "api/design/list";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            DesignListResponse designListResponse = new DesignListResponse();
            designListResponse.RealJsonstring = resjosn;
            return designListResponse;
        }
        /// <summary>
        /// 此接口返回第三方用户作品列表
        /// </summary>
        /// <param name="order">排序 1最新创建 2最近修改</param>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public DesignListResponse DesignList(string order, string? page = "", string? page_size = "")
        {
            Task<DesignListResponse> task = DesignListAsync(order, page, page_size);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 23.作品详情
        /// <summary>
        /// 此接口返回第三方用户作品详情 ASYNC
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public async Task<DesignInfoResponse> DesignInfoAsync(string user_design_id)
        {
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("user_design_id", user_design_id);
            string url = Url + "api/design/info";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            DesignInfoResponse designInfoResponse = new DesignInfoResponse();
            designInfoResponse.RealJsonstring = resjosn;
            return designInfoResponse;
        }
        /// <summary>
        /// 此接口返回第三方用户作品详情
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public DesignInfoResponse DesignInfo(string user_design_id)
        {
            Task<DesignInfoResponse> task = DesignInfoAsync(user_design_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 24.作品导出
        /// <summary>
        /// 此接口会根据传入的参数生成一条导出任务并返回任务标识，需结合 导出结果 接口轮询返回结果。ASYNC
        /// </summary>
        /// <param name="id">作品ID</param>
        /// <param name="format">导出格式:png|jpeg|pdf|ppt</param>
        /// <param name="edit">导出的作品是否可编辑
        ///true=可编辑
        ///false=不可编辑</param>
        /// <param name="files_to_zip">导出的图片是否压缩为zip
        /// true=不压缩
        /// false=压缩
        /// </param>
        /// <returns></returns>
        public async Task<ExportFileResponse> ExportFileAsync(string id, string format = "ppt", string edit = "true", string files_to_zip = "false")
        {
            string url = Url + "api/download/export/file";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("id", id));
            UrlEncoded.Add(new KeyValuePair<string, string>("format", format));
            UrlEncoded.Add(new KeyValuePair<string, string>("edit", edit));
            UrlEncoded.Add(new KeyValuePair<string, string>("files_to_zip", files_to_zip));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            ExportFileResponse exportFileResponse = new ExportFileResponse();
            exportFileResponse.RealJsonstring = resjosn;
            return exportFileResponse;
        }
        /// <summary>
        /// 此接口会根据传入的参数生成一条导出任务并返回任务标识，需结合 导出结果 接口轮询返回结果。
        /// </summary>
        /// <param name="id">作品ID</param>
        /// <param name="format">导出格式:png|jpeg|pdf|ppt</param>
        /// <param name="edit">导出的作品是否可编辑
        ///true=可编辑
        ///false=不可编辑</param>
        /// <param name="files_to_zip">导出的图片是否压缩为zip
        /// true=不压缩
        /// false=压缩
        /// </param>
        /// <returns></returns>
        public ExportFileResponse ExportFile(string id, string format = "ppt", string edit = "true", string files_to_zip = "false")
        {
            Task<ExportFileResponse> task = ExportFileAsync(id, format, edit, files_to_zip);
            task.Wait();
            return task.Result;
        }

        #endregion

        #region 25.作品导出结果
        /// <summary>
        /// 此接口需要根据 作品导出 接口返回的任务标识轮询查询结果 ASYNC
        /// </summary>
        /// <param name="task_key">作品导出的任务标识</param>
        /// <returns></returns>
        public async Task<DownloadExportFileResponse> DownloadExportFileAsync(string task_key)
        {
            string url = Url + "api/download/export/file/result";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("task_key", task_key));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            DownloadExportFileResponse downloadExportFileResponse= new DownloadExportFileResponse();
            downloadExportFileResponse.RealJsonstring = resjosn;
            return downloadExportFileResponse;
        }

        /// <summary>
        /// 此接口需要根据 作品导出 接口返回的任务标识轮询查询结果
        /// </summary>
        /// <param name="task_key">作品导出的任务标识</param>
        /// <returns></returns>
        public DownloadExportFileResponse DownloadExportFile(string task_key)
        {
            Task<DownloadExportFileResponse> task = DownloadExportFileAsync(task_key);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 26.作品重命名
        /// <summary>
        /// 第三方用户修改作品名称 ASYNC
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <param name="name">作品名称</param>
        /// <returns></returns>
        public async Task<SaveNameResponse> SaveNameAsync(string user_design_id,string name)
        {
            string url = Url + "api/design/save/name";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("user_design_id", user_design_id));
            UrlEncoded.Add(new KeyValuePair<string, string>("name", name));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            SaveNameResponse saveNameResponse= new SaveNameResponse();
            saveNameResponse.RealJsonstring = resjosn;
            return saveNameResponse;
        }

        /// <summary>
        /// 第三方用户修改作品名称
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <param name="name">作品名称</param>
        /// <returns></returns>
        public SaveNameResponse SaveName(string user_design_id, string name)
        {
            Task<SaveNameResponse> task = SaveNameAsync(user_design_id, name);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 27.作品删除
        /// <summary>
        /// 第三方用户删除作品 ASYNC
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public async Task<DesignDeleteResponse> DesignDeleteAsync(string user_design_id)
        {
            string url = Url + "api/design/delete";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("user_design_id", user_design_id));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            DesignDeleteResponse designDeleteResponse = new DesignDeleteResponse();
            designDeleteResponse.RealJsonstring = resjosn;
            return designDeleteResponse;
        }
        /// <summary>
        /// 第三方用户删除作品
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public DesignDeleteResponse DesignDelete(string user_design_id)
        {
            Task<DesignDeleteResponse> task = DesignDeleteAsync(user_design_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 28.已删除列表
        /// <summary>
        /// 此接口返回第三方用户已删除进入回收站的作品列表 ASYNC
        /// </summary>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public async Task<DesignDelListResponse> DesignDelListAsync(string? page = "", string? page_size = "")
        {
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(page))
            {
                QuerykeyValuePairs.Add("page", page!);
            }
            if (!string.IsNullOrWhiteSpace(page_size))
            {
                QuerykeyValuePairs.Add("page_size", page_size!);
            }

            string url = Url + "api/design/del/list";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            DesignDelListResponse designDelListResponse = new DesignDelListResponse();
            designDelListResponse.RealJsonstring = resjosn;
            return designDelListResponse;
        }

        /// <summary>
        /// 此接口返回第三方用户已删除进入回收站的作品列表 
        /// </summary>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public DesignDelListResponse DesignDelList(string? page = "", string? page_size = "")
        {
            Task<DesignDelListResponse> task = DesignDelListAsync(page, page_size);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 29.作品还原
        /// <summary>
        /// 第三方用户已删除作品还原 ASYNC
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public async Task<DesignRevertResponse> DesignRevertAsync(string user_design_id)
        {
            string url = Url + "api/design/revert";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("user_design_id", user_design_id));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            DesignRevertResponse designRevertResponse = new DesignRevertResponse();
            designRevertResponse.RealJsonstring = resjosn;
            return designRevertResponse;
        }
        /// <summary>
        /// 第三方用户已删除作品还原
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public DesignRevertResponse DesignRevert(string user_design_id)
        {
            Task<DesignRevertResponse> task = DesignRevertAsync(user_design_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 30.彻底删除
        /// <summary>
        /// 第三方用户已删除作品彻底删除 ASYNC
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public async Task<DesignClearResponse> DesignClearAsync(string user_design_id)
        {
            string url = Url + "api/design/clear";
            List<KeyValuePair<string, string>> UrlEncoded = new List<KeyValuePair<string, string>>();
            UrlEncoded.Add(new KeyValuePair<string, string>("user_design_id", user_design_id));
            string resjosn = await SendRequestAsync(method: HttpMethod.Post, url: url, body_UrlEncoded: UrlEncoded);
            DesignClearResponse designClearResponse = new DesignClearResponse();
            designClearResponse.RealJsonstring = resjosn;
            return designClearResponse;
        }
        /// <summary>
        /// 第三方用户已删除作品彻底删除
        /// </summary>
        /// <param name="user_design_id">作品ID</param>
        /// <returns></returns>
        public DesignClearResponse DesignClear(string user_design_id)
        {
            Task<DesignClearResponse> task = DesignClearAsync(user_design_id);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 31.预置词列表
        /// <summary>
        /// 预置词列表中的数据，可以调用预置词详情API获取可以直接生成PPT的数据 ASYNC
        /// </summary>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public async Task<ConfigListResponse> ConfigListAsync(string? page = "", string? page_size = "")
        {
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(page))
            {
                QuerykeyValuePairs.Add("page", page!);
            }
            if (!string.IsNullOrWhiteSpace(page_size))
            {
                QuerykeyValuePairs.Add("page_size", page_size!);
            }

            string url = Url + "api/ai/chat/config/list";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            ConfigListResponse configListResponse = new ConfigListResponse();
            configListResponse.RealJsonstring = resjosn;
            return configListResponse;
        }
        /// <summary>
        /// 预置词列表中的数据，可以调用预置词详情API获取可以直接生成PPT的数据
        /// </summary>
        /// <param name="page">页码 默认1</param>
        /// <param name="page_size">每页展示数量 默认20</param>
        /// <returns></returns>
        public ConfigListResponse ConfigList(string? page = "", string? page_size = "")
        {
            Task<ConfigListResponse> task = ConfigListAsync(page, page_size);
            task.Wait();
            return task.Result;
        }
        #endregion

        #region 32.预置词详情
        /// <summary>
        /// 用过预置词列表中的预置词ID，获取可以直接生成PPT的数据 ASYNC
        /// </summary>
        /// <param name="id">预置词ID</param>
        /// <returns></returns>
        public async Task<ConfigDetailResponse> ConfigDetailAsync(string id)
        {
            Dictionary<string, string> QuerykeyValuePairs = new Dictionary<string, string>();
            QuerykeyValuePairs.Add("id", id!);
            string url = Url + "api/ai/chat/config/detail";
            string resjosn = await SendRequestAsync(method: HttpMethod.Get, url: url, queryParameters: QuerykeyValuePairs);
            ConfigDetailResponse configDetailResponse = new ConfigDetailResponse();
            configDetailResponse.RealJsonstring = resjosn;
            return configDetailResponse;
        }

        /// <summary>
        /// 用过预置词列表中的预置词ID，获取可以直接生成PPT的数据
        /// </summary>
        /// <param name="id">预置词ID</param>
        /// <returns></returns>
        public ConfigDetailResponse ConfigDetail(string id)
        {
            Task<ConfigDetailResponse> task = ConfigDetailAsync(id);
            task.Wait();
            return task.Result;
        }
        #endregion
    }
}
