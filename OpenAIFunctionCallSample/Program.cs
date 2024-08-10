using Azure.AI.OpenAI;
using Azure;
using OpenAIFunctionCallSample.Models;
using System.Text.Json;
using OpenAI.Chat;

namespace OpenAIFunctionCallSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var apikey = "{your api key}";
            var apiUrl = "https://{your openai endpoint}.openai.azure.com";
            string deploymentName = "{your deployment name}";

            var chatCompletionsOptions = new ChatCompletionOptions();

            foreach (var item in ChatTools.GetChatTools())
            {
                chatCompletionsOptions.Tools.Add(item);
            }

            List<ChatMessage> conversationMessages =
            [
                //new UserChatMessage("我想要點兩份牛排"),
                //new UserChatMessage("請問台北市的天氣?"),
                new UserChatMessage("請問 PS5 多少錢?"),
            ];

            var client = new AzureOpenAIClient(new Uri(apiUrl), new AzureKeyCredential(apikey)).GetChatClient(deploymentName);
            ChatCompletion completion = client.CompleteChat(conversationMessages, options: chatCompletionsOptions);

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                foreach (var toolCall in completion.ToolCalls)
                {
                    switch (toolCall.FunctionName)
                    {
                        case "CalFoodPrice":
                            var foodInfo = JsonSerializer.Deserialize<FoodInfo>(toolCall.FunctionArguments);
                            var foodPrice = Functions.CalFoodPrice(foodInfo);
                            Console.WriteLine($"您點了 {foodInfo.Count} 份 {foodInfo.Food} 總共 {foodPrice} 元");
                            break;
                        case "GetCurrentWeather":
                            var weatherConfig = JsonSerializer.Deserialize<WeatherConfig>(toolCall.FunctionArguments);
                            Console.WriteLine(Functions.GetCurrentWeather(weatherConfig));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Console.Write(completion);
            }
        }
    }
}