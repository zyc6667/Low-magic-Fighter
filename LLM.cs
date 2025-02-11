namespace ZYC;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
//using AlibabaCloud.SDK.Bailian20231229;
//using AlibabaCloud.SDK.Bailian20231229.Models;

public class LLM
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task<string> SendPostRequestAsync(string url, string jsonContent, string apiKey)
    {
        using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
        {
            // 设置请求头
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 发送请求并获取响应
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            // 处理响应
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"请求失败: {response.StatusCode}";
            }
        }
    }

    public static string GetContentFromJson(string json)
    {
        // 解析 JSON 数据
        var jsonObject = JObject.Parse(json);

        // 提取 content 字段
        string content = jsonObject["choices"]?.Value<JToken>(0)?["message"]?.Value<string>("content") ?? "解码错误";

        return content;
    }
}
