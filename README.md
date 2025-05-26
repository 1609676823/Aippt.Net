# Aippt.Net
AIPPT�ӿڹٷ��ĵ�:https://open.aippt.cn/docs/zh/guide/introduce.html
# 1.��ȡ�߼�����
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SeniorOptionResponse seniorOptionResponse = aipptClient.GetSeniorOption();
Console.WriteLine(seniorOptionResponse.RealJsonstring);
Console.WriteLine(seniorOptionResponse.data!.FirstOrDefault()!.id);
```
# 2.���񴴽�
����ʾ��:
```����ʾ��
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git��֧����";
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
```
�ļ�ʾ��:
```�ļ�ʾ��
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest=new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadTxt;
taskCreateRequest.title = "git����淶";
taskCreateRequest.file = new TaskFile("demo.txt") { FilePath= "demo.txt" };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
```
# 3.����������
## 3.1 ��������
### Ĭ��AI����
#### 3.11 ����1.�������ɴ��
��ʽ���Ļ�ȡ��ʽ:
```��ʽ���Ļ�ȡ��ʽ
// ����������ȴ�
Console.TreatControlCAsInput = true;
var exit = new ManualResetEvent(false);
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
aipptClient.CreateOutlineAsync(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) => 
{
    Console.Write(response.content);
}
);
/*��Ҫ�����̲߳��˳����ܻ�ȡ��*/
exit.WaitOne();
// while (true) {}
```
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string Outline = aipptClient.CreateOutline(task_id: task_id);
```
#### 3.12 ����2.����������� (��)
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ContentResponse contentResponse = aipptClient.CreateContent(task_id: task_id);
string ticket = contentResponse.data!;
```
#### 3.13 ����3.����������ݽ����ѯ (��)
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
CheckContentResponse checkContentResponse = aipptClient.CheckContent(ticket: "ticket");
Console.WriteLine(checkContentResponse.data!.status);
Console.WriteLine(checkContentResponse.data!.content);
```
### �ٶ�AI����
#### 3.14 ����1.�������ɴ��
��ʽ���Ļ�ȡ��ʽ:
```��ʽ���Ļ�ȡ��ʽ
// ����������ȴ�
Console.TreatControlCAsInput = true;
var exit = new ManualResetEvent(false);
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
aipptClient.CreateWxOutline(task_id: task_id);
aipptClient.ObservMessageReceived.OfType<MeaasgeResponse>().Subscribe((response) => 
{
    Console.Write(response.content);
}
);
/*��Ҫ�����̲߳��˳����ܻ�ȡ��*/
exit.WaitOne();
// while (true) {}
```
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string Outline = aipptClient.CreateWxOutline(task_id: task_id);
```
#### 3.15 ����2.�����������
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ContentResponse contentResponse = aipptClient.CreateWxContent(task_id: task_id);
string ticket = contentResponse.data!;
```
## 3.2 �ϴ�Word
��ʽ���Ļ�ȡ��ʽ:
```��ʽ���Ļ�ȡ��ʽ
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
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
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
## 3.3 �ϴ��ο��ĵ�
��ʽ���Ļ�ȡ��ʽ:
```��ʽ���Ļ�ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadReferenceDocument;
taskCreateRequest.title = "git����淶";
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
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.UploadReferenceDocument;
taskCreateRequest.title = "git����淶";
taskCreateRequest.files =new List<TaskFile>() { new TaskFile("demo.docx") { FilePath = "demo.docx" } };
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string OutlineContent = aipptClient.CreateReferOutlineContent(task_id);
```
## 3.4 �ϴ���ͨ�ļ�
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
## 3.5 ������������
```��ʽ���Ļ�ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git��֧����";
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
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
taskCreateRequest.type = TaskCreateType.SmartGenerate;
taskCreateRequest.title = "git��֧����";
taskCreateRequest.is_web_search = true;
TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
string task_id = taskCreateResponse.data!.id.ToString();
Console.WriteLine(taskCreateResponse.RealJsonstring);
Console.WriteLine(taskCreateResponse.data!.id);
string NetworkOutlineContent= aipptClient.CreateNetworkOutlineContent(task_id: task_id);
```
## 3.6 ����URL����
��ʽ���Ļ�ȡ��ʽ:
```��ʽ���Ļ�ȡ��ʽ
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
ͬ����ȡ��ʽ:
```ͬ����ȡ��ʽ
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
# 4.��ȡPPT���ݽṹ
## 4.1 ����ID��ȡPPT���νṹ
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
PptTreeResponse pptTreeResponse = aipptClient.GetPptTreeByTaskid(task_id: task_id);
Console.WriteLine(pptTreeResponse.RealJsonstring);
Console.WriteLine(pptTreeResponse.data!.depth);
```
## 4.2 ��ƷID��ȡPPT���νṹ
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
PptTreeResponse pptTreeResponse = aipptClient.GetPptTreeByDesignid(design_id: design_id);
Console.WriteLine(pptTreeResponse.RealJsonstring);
Console.WriteLine(pptTreeResponse.data!.depth);
```
# 5.������١���������
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
GenerateFileResponse generateFileResponse = aipptClient.GetGenerateFile(task_id: task_id, type:type);
Console.WriteLine(generateFileResponse.RealJsonstring);
Console.WriteLine(generateFileResponse.data);
```
# 6.��ȡ�û���ٴ���
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
UserTaskResponse userTaskResponse = aipptClient.GetUserTask(new List<string>() { "userid"});
Console.WriteLine(userTaskResponse.RealJsonstring);
Console.WriteLine(userTaskResponse.data!.FirstOrDefault()!.count);
```
# 7.�༭���
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
string content = "{\"children\":[{\"children\":[],\"depth\":2,\"direction\":1,\"expanded\":true,\"id\":2,\"pageIndex\":2,\"parentId\":1,\"showTip\":false,\"sort\":0,\"type\":\"catalog\",\"value\":\"Ŀ¼\"},{\"children\":[],\"depth\":2,\"direction\":1,\"expanded\":true,\"id\":3,\"pageIndex\":3,\"parentId\":1,\"showTip\":false,\"sort\":1,\"type\":\"ending\",\"value\":\"����\"}],\"depth\":1,\"direction\":1,\"expanded\":true,\"id\":1,\"index\":0,\"lastLevel\":true,\"pageIndex\":1,\"parentId\":0,\"showTip\":false,\"sort\":0,\"type\":\"title\",\"value\":\"����#�ٺٺ�#��������-����1-����2\"}";
OutlineSaveResponse outlineSaveResponse = aipptClient.OutlineSave(task_id: "task_id", content:content);
Console.WriteLine(outlineSaveResponse.RealJsonstring);
Console.WriteLine(outlineSaveResponse.data);
```
# 8.ģ��
## 8.1 ģ����װ�б�ɸѡ��
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SuitSelectResponse suitSelectResponse =  aipptClient.GetSuitSelect();
Console.WriteLine(suitSelectResponse.RealJsonstring);
Console.WriteLine(suitSelectResponse.data!.colour.FirstOrDefault()!.id);
```
## 8.2 ģ����װ�б�
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
## 8.3 ��ҵģ����װ�б�
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SuitSearchResponse suitSearchResponse =  aipptClient.EnterpriseSuitSearch();
Console.WriteLine(suitSearchResponse.RealJsonstring);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.id);
Console.WriteLine(suitSearchResponse.data!.list!.FirstOrDefault()!.cover_img);
```
# 9.��Ʒ
## 9.1 ��Ʒ����
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignSaveResponse designSaveResponse =  aipptClient.DesignSave(task_id: "task_id", template_id: "template_id");
Console.WriteLine(designSaveResponse.RealJsonstring);
Console.WriteLine(designSaveResponse.data!.id);
Console.WriteLine(designSaveResponse.data!.name);
Console.WriteLine(designSaveResponse.data!.cover_url);
Console.WriteLine(designSaveResponse.data!.size);
```

## 9.2 ��Ʒ�б�
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignListResponse designListResponse = aipptClient.DesignList(order: "1");
Console.WriteLine(designListResponse.RealJsonstring);
Console.WriteLine(designListResponse.data!.list!.Count);
```
## 9.3 ��Ʒ����
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignInfoResponse designInfoResponse = aipptClient.DesignInfo(user_design_id: "user_design_id");
Console.WriteLine(designInfoResponse.RealJsonstring);
Console.WriteLine(designInfoResponse.data!.id);
```
## 9.4 ��Ʒ����
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
ExportFileResponse exportFileResponse= aipptClient.ExportFile(id: "design_id"); 
Console.WriteLine(exportFileResponse.RealJsonstring);
Console.WriteLine(exportFileResponse.data);/*task_key��Ʒ�����������ʶ*/
```
## 9.5 ��Ʒ�������
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DownloadExportFileResponse downloadExportFileResponse = aipptClient.DownloadExportFile(task_key: "task_key");
Console.WriteLine(downloadExportFileResponse.RealJsonstring);
Console.WriteLine(downloadExportFileResponse.data!.FirstOrDefault());

```
## 9.6 ��Ʒ������
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
SaveNameResponse  saveNameResponse= aipptClient.SaveName(user_design_id: "user_design_id", name: "��Ʒ������");
Console.WriteLine(saveNameResponse.RealJsonstring);
Console.WriteLine(saveNameResponse.data!.name);
```
## 9.7 ��Ʒɾ��
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignDeleteResponse designDeleteResponse = aipptClient.DesignDelete(user_design_id: "user_design_id");
Console.WriteLine(designDeleteResponse.RealJsonstring);
Console.WriteLine(designDeleteResponse.data!);
```
# 10.����վ
## 10.1 ��ɾ���б�
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignDelListResponse designDelListResponse= aipptClient.DesignDelList();
Console.WriteLine(designDelListResponse.RealJsonstring);
Console.WriteLine(designDelListResponse.data!.list!.FirstOrDefault()!.id);
Console.WriteLine(designDelListResponse.data!.list!.FirstOrDefault()!.name);
```
## 10.2 ��Ʒ��ԭ
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignRevertResponse designRevertResponse = aipptClient.DesignRevert(user_design_id: "user_design_id");
Console.WriteLine(designRevertResponse.RealJsonstring);
Console.WriteLine(designRevertResponse.data!);
```

## 10.3 ����ɾ��
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
DesignClearResponse designClearResponse = aipptClient.DesignClear(user_design_id: "user_design_id");
Console.WriteLine(designClearResponse.RealJsonstring);
Console.WriteLine(designClearResponse.data!);
Console.WriteLine(designClearResponse.msg);
```

# 11.Ԥ�ô�
## 11.1 Ԥ�ô��б�
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
```
## 11.2 Ԥ�ô�����
```
AipptClient aipptClient=new AipptClient("apikey", "secretkey","userid");
```
    