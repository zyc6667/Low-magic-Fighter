using System;
using System.Text;

namespace ZYC
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch(); 
            oTime.Start(); //记录开始时间

            int myNum=Tool.GetAbs(-10);
            int itNum=Math.Abs(-10);

            double Xn=Tool.DiTui(Math.PI/4,10);
            Console.WriteLine(Xn);

            double multiple=Tool.CunKuan(0.025,30); //30年期国债，利率2.5%
            Console.WriteLine(multiple);

            oTime.Stop(); //记录结束时间
            Console.WriteLine("程序的运行时间：{0} 毫秒", oTime.Elapsed.Milliseconds); //输出运行时间
        }
        */
        static async Task Main(string[] args)
        {
            // 设置控制台输出编码为 UTF-8
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.InputEncoding = Encoding.GetEncoding("GB2312");
            Console.OutputEncoding = Encoding.GetEncoding("GB2312");
            // 若没有配置环境变量，请用百炼API Key将下行替换为：string? apiKey = "sk-xxx";
            //string? apiKey = Environment.GetEnvironmentVariable("DASHSCOPE_API_KEY");
            string? apiKey = "sk-aa8fb68f9953487cb95362a6e696e149";

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API Key 未设置。请确保环境变量 'DASHSCOPE_API_KEY' 已设置。");
                return;
            }

            Console.WriteLine("请输入问题：");
            // 等待用户输入并获取输入的字符串
            string prompt = Console.ReadLine()??"输入错误";

            string url = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";
            // 模型列表：https://help.aliyun.com/zh/model-studio/getting-started/models

            string jsonContent = $@"{{
            ""model"": ""qwen-plus"",
            ""messages"": [
                {{
                    ""role"": ""system"",
                    ""content"": ""You are a helpful assistant.""
                }},
                {{
                    ""role"": ""user"", 
                    ""content"": ""{prompt}""
                }}
                ]
            }}";

            // 发送请求并获取响应
            string result = await LLM.SendPostRequestAsync(url, jsonContent, apiKey);
            string resultToCustomer = LLM.GetContentFromJson(result);
            // 输出结果
            Console.WriteLine(resultToCustomer);
            Console.ReadLine();
        }
    }
}
