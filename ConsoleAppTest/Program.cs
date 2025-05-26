using Aippt.Net;
using Aippt.Net.Enum;
using Aippt.Net.Model;
using System.Reactive.Linq;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
namespace ConsoleAppTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 禁用鼠标点击等待
            Console.TreatControlCAsInput = true;
            var exit = new ManualResetEvent(false);
            AipptClient aipptClient=new AipptClient("X", "X","user");

            //CodeResponse codeResponse= aipptClient.GetCode();
            Dictionary<string, string> dicAuthHeaders = aipptClient.GetApiAuthHeadersByToken();
            foreach (var item in dicAuthHeaders)
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }

            ConfigDetailResponse configDetailResponse = aipptClient.ConfigDetail("1");
            Console.WriteLine(configDetailResponse.RealJsonstring);
            Console.WriteLine(configDetailResponse.data!.id);
            Console.WriteLine(configDetailResponse.data!.title);
            Console.WriteLine(configDetailResponse.data!.content);

            //ExportFileResponse exportFileResponse= aipptClient.ExportFile(id: "42685536",files_to_zip:"true");
            //string task_key = exportFileResponse.data!;
            //DesignDeleteResponse designDeleteResponse = aipptClient.DesignDelete(user_design_id: "42685536");
            //Console.WriteLine(designDeleteResponse.RealJsonstring);
            //Console.WriteLine(designDeleteResponse.data!);

            ///*任务创建*/
            //TaskCreateRequest taskCreateRequest = new TaskCreateRequest();
            //taskCreateRequest.type = TaskCreateType.SmartGenerate;
            //taskCreateRequest.title = "git分支管理";
            //TaskCreateResponse taskCreateResponse = aipptClient.TaskCreate(taskCreateRequest);
            //string task_id = taskCreateResponse.data!.id.ToString();

            ///*步骤1.标题生成大纲*/
            //string Outline = aipptClient.CreateOutline(task_id: task_id);

            ///*步骤2.大纲生成内容 (新)*/
            //ContentResponse contentResponse = aipptClient.CreateContent(task_id: task_id);
            //string ticket = contentResponse.data!;

            //int status = 1;
            //CheckContentResponse? checkContentResponse = null;
            //while (true)
            //{
            //    checkContentResponse = aipptClient.CheckContent(ticket: ticket);
            //    //Console.WriteLine(checkContentResponse.data!.status);
            //    //Console.WriteLine(checkContentResponse.data!.content);
            //    status = checkContentResponse.data!.status;
            //    if (status != 1)
            //    {
            //        break;
            //    }
            //}
            //Console.WriteLine(checkContentResponse.data!.status);
            //Console.WriteLine(checkContentResponse.data!.content);

            /*任务ID获取PPT树形结构*/
            //PptTreeResponse pptTreeResponse = aipptClient.GetPptTreeByDesignid(design_id: design_id);
            //Console.WriteLine(pptTreeResponse.RealJsonstring);
            //Console.WriteLine(pptTreeResponse.data!.depth);

            //  Console.WriteLine("Hello, World!");
            // Console.ReadKey();
            /*需要保持线程不退出才能获取到*/
            exit.WaitOne();
            // while (true) {}
        }
    }
}
