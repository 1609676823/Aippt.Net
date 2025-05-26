# Aippt.Net
AIPPT接口官方文档:https://open.aippt.cn/docs/zh/guide/introduce.html
# 1.获取高级配置
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SeniorOptionResponse seniorOptionResponse = aipptClient.GetSeniorOption();
Console.WriteLine(seniorOptionResponse.RealJsonstring);
Console.WriteLine(seniorOptionResponse.data!.FirstOrDefault()!.id);
```
# 2.任务创建
标题示例:
```标题示例
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git分支管理";
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
```
文件示例:
```文件示例
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest=new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadTxt;
taskCreateRequest.title = "git代码规范";
taskCreateRequest.file = new TaskFile("demo.txt") { FilePath= "demo.txt" };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
```
# 3.创建任务结果
## 3.1 智能生成
### 默认AI生成
#### 3.11 步骤1.标题生成大纲
流式订阅获取方式:
```流式订阅获取方式
// 禁用鼠标点击等待
Console.TreatControlCAsInput = true;
var exit = new ManualResetEvent(false);
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
aipptClient.CreateOutlineAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) => 
{
    Console.Write(response.content);
}
);
/*需要保持线程不退出才能获取到*/
exit.WaitOne();
// while (true) {}
```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string Outline = aipptClient.CreateOutline(task_id: task_id);
```
#### 3.12 步骤2.大纲生成内容 (新)
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ContentResponse contentResponse = aipptClient.CreateContent(task_id: task_id);
string ticket = contentResponse.data!;
```
#### 3.13 步骤3.大纲生成内容结果查询 (新)
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
CheckContentResponse checkContentResponse = aipptClient.CheckContent(ticket: "ticket");
Console.WriteLine(checkContentResponse.data!.status);
Console.WriteLine(checkContentResponse.data!.content);
```
### 百度AI生成
#### 3.14 步骤1.标题生成大纲
流式订阅获取方式:
```流式订阅获取方式
// 禁用鼠标点击等待
Console.TreatControlCAsInput = true;
var exit = new ManualResetEvent(false);
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
aipptClient.CreateWxOutline(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) => 
{
    Console.Write(response.content);
}
);
/*需要保持线程不退出才能获取到*/
exit.WaitOne();
// while (true) {}
```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string Outline = aipptClient.CreateWxOutline(task_id: task_id);
```
#### 3.15 步骤2.大纲生成内容
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ContentResponse contentResponse = aipptClient.CreateWxContent(task_id: task_id);
string ticket = contentResponse.data!;
```
## 3.2 上传Word
流式订阅获取方式:
```流式订阅获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadWord;
taskCreateRequest.file = new TaskFile("demo.docx") { FilePath = "demo.docx" };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
aipptClient.CreateWordOutlineContentAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) =>
{
    Console.Write(response.content);
}
);

```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
 TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadWord;
taskCreateRequest.file = new TaskFile("demo.docx") { FilePath = "demo.docx" };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string OutlineContent= aipptClient.CreateWordOutlineContent(task_id);
```
## 3.3 上传参考文档
流式订阅获取方式:
```流式订阅获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadReferenceDocument;
taskCreateRequest.title = "git代码规范";
taskCreateRequest.files =new List<TaskFile>() { new TaskFile("demo.docx") { FilePath = "demo.docx" } };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
aipptClient.CreateReferOutlineContentAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) =>
{
    Console.Write(response.content);
}
);
```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadReferenceDocument;
taskCreateRequest.title = "git代码规范";
taskCreateRequest.files =new List<TaskFile>() { new TaskFile("demo.docx") { FilePath = "demo.docx" } };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string OutlineContent = aipptClient.CreateReferOutlineContent(task_id);
```
## 3.4 上传普通文件
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
 TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadMarkdown;
taskCreateRequest.file = new TaskFile("demo.md") { FilePath = "demo.md" };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
MarkdownParseResponse  markdownParseResponse= aipptClient.CreateConverFileMarkdown(task_id, "6");
Console.WriteLine(markdownParseResponse.RealJsonstring);
Console.WriteLine(markdownParseResponse.data);
```
## 3.5 联网智能生成
```流式订阅获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git分支管理";
taskCreateRequest.is_web_search = true;
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
aipptClient.CreateNetworkOutlineContentAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) =>
{
    Console.Write(response.content);
}
);

```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git分支管理";
taskCreateRequest.is_web_search = true;
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string NetworkOutlineContent= aipptClient.CreateNetworkOutlineContent(task_id: task_id);
```
## 3.6 导入URL链接
流式订阅获取方式:
```流式订阅获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.ImportUrlLink;
taskCreateRequest.link = "https://open.aippt.cn/docs/zh/guide/introduce.html";
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
aipptClient.CreateLinkOutlineContentAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) =>
{
    Console.Write(response.content);
}
);

```
同步获取方式:
```同步获取方式
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.ImportUrlLink;
taskCreateRequest.link = "https://open.aippt.cn/docs/zh/guide/introduce.html";
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string LinkOutlineContent = aipptClient.CreateLinkOutlineContent(task_id: task_id);
```
# 4.获取PPT数据结构
## 4.1 任务ID获取PPT树形结构
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
PptTreeResponse pptTreeResponse = aipptClient.GetPptTreeByTaskid(task_id: task_id);
Console.WriteLine(pptTreeResponse.RealJsonstring);
Console.WriteLine(pptTreeResponse.data!.depth);
```
## 4.2 作品ID获取PPT树形结构
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
PptTreeResponse pptTreeResponse = aipptClient.GetPptTreeByDesignid(design_id: design_id);
Console.WriteLine(pptTreeResponse.RealJsonstring);
Console.WriteLine(pptTreeResponse.data!.depth);
```
# 5.导出大纲、完整内容
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
GenerateFileResponse generateFileResponse = aipptClient.GetGenerateFile(task_id: task_id, type:type);
Console.WriteLine(generateFileResponse.RealJsonstring);
Console.WriteLine(generateFileResponse.data);
```
# 6.获取用户大纲次数
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
UserTaskResponse userTaskResponse = aipptClient.GetUserTask(new List<string>() { "userid"});
Console.WriteLine(userTaskResponse.RealJsonstring);
Console.WriteLine(userTaskResponse.data!.FirstOrDefault()!.count);
```
# 7.编辑大纲
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string content = "{\"children\":[{\"children\":[],\"depth\":2,\"direction\":1,\"expanded\":true,\"id\":2,\"pageIndex\":2,\"parentId\":1,\"showTip\":false,\"sort\":0,\"type\":\"catalog\",\"value\":\"目录\"},{\"children\":[],\"depth\":2,\"direction\":1,\"expanded\":true,\"id\":3,\"pageIndex\":3,\"parentId\":1,\"showTip\":false,\"sort\":1,\"type\":\"ending\",\"value\":\"结语\"}],\"depth\":1,\"direction\":1,\"expanded\":true,\"id\":1,\"index\":0,\"lastLevel\":true,\"pageIndex\":1,\"parentId\":0,\"showTip\":false,\"sort\":0,\"type\":\"title\",\"value\":\"哈哈#嘿嘿嘿#嘻嘻嘻嘻-正文1-正文2\"}";
OutlineSaveResponse outlineSaveResponse = aipptClient.OutlineSave(task_id: "task_id", content:content);
Console.WriteLine(outlineSaveResponse.RealJsonstring);
Console.WriteLine(outlineSaveResponse.data);
```
# 8.模板
## 8.1 模板套装列表筛选项
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SuitSelectResponse suitSelectResponse =  aipptClient.GetSuitSelect();
Console.WriteLine(suitSelectResponse.RealJsonstring);
Console.WriteLine(suitSelectResponse.data!.colour.FirstOrDefault()!.id);
```
## 8.2 模板套装列表
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SuitSearchRequest suitSearchRequest = new SuitSearchRequest();
suitSearchRequest.style_id = 2;
suitSearchRequest.colour_id = 7;
SuitSearchResponse suitSearchResponse =  aipptClient.SuitSearch(suitSearchRequest);
Console.WriteLine(suitSearchResponse.RealJsonstring);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.id);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.cover_img);
```
## 8.3 企业模板套装列表
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SuitSearchResponse suitSearchResponse =  aipptClient.EnterpriseSuitSearch();
Console.WriteLine(suitSearchResponse.RealJsonstring);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.id);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.cover_img);
```
# 9.作品
## 9.1 作品生成
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignSaveResponse designSaveResponse =  aipptClient.DesignSave(task_id: "task_id", template_id: "template_id");
Console.WriteLine(designSaveResponse.RealJsonstring);
Console.WriteLine(designSaveResponse.data!.id);
Console.WriteLine(designSaveResponse.data!.name);
Console.WriteLine(designSaveResponse.data!.cover_url);
Console.WriteLine(designSaveResponse.data!.size);
```

## 9.2 作品列表
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignListResponse designListResponse = aipptClient.DesignList(order: "1");
Console.WriteLine(designListResponse.RealJsonstring);
Console.WriteLine(designListResponse.data!.list!.Count);
```
## 9.3 作品详情
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignInfoResponse designInfoResponse = aipptClient.DesignInfo(user_design_id: "user_design_id");
Console.WriteLine(designInfoResponse.RealJsonstring);
Console.WriteLine(designInfoResponse.data!.id);
```
## 9.4 作品导出
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ExportFileResponse exportFileResponse= aipptClient.ExportFile(id: "design_id"); 
Console.WriteLine(exportFileResponse.RealJsonstring);
Console.WriteLine(exportFileResponse.data);/*task_key作品导出的任务标识*/
```
## 9.5 作品导出结果
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DownloadExportFileResponse downloadExportFileResponse = aipptClient.DownloadExportFile(task_key: "task_key");
Console.WriteLine(downloadExportFileResponse.RealJsonstring);
Console.WriteLine(downloadExportFileResponse.data!.FirstOrDefault());

```
## 9.6 作品重命名
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SaveNameResponse  saveNameResponse= aipptClient.SaveName(user_design_id: "user_design_id", name: "作品重命名");
Console.WriteLine(saveNameResponse.RealJsonstring);
Console.WriteLine(saveNameResponse.data!.name);
```
## 9.7 作品删除
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignDeleteResponse designDeleteResponse = aipptClient.DesignDelete(user_design_id: "user_design_id");
Console.WriteLine(designDeleteResponse.RealJsonstring);
Console.WriteLine(designDeleteResponse.data!);
```
# 10.回收站
## 10.1 已删除列表
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignDelListResponse designDelListResponse= aipptClient.DesignDelList();
Console.WriteLine(designDelListResponse.RealJsonstring);
Console.WriteLine(designDelListResponse.data!.list!.FirstOrDefault()!.id);
Console.WriteLine(designDelListResponse.data!.list!.FirstOrDefault()!.name);
```
## 10.2 作品还原
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignRevertResponse designRevertResponse = aipptClient.DesignRevert(user_design_id: "user_design_id");
Console.WriteLine(designRevertResponse.RealJsonstring);
Console.WriteLine(designRevertResponse.data!);
```

## 10.3 彻底删除
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignClearResponse designClearResponse = aipptClient.DesignClear(user_design_id: "user_design_id");
Console.WriteLine(designClearResponse.RealJsonstring);
Console.WriteLine(designClearResponse.data!);
Console.WriteLine(designClearResponse.msg);
```

# 11.预置词
## 11.1 预置词列表
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
```
## 11.2 预置词详情
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
```
    