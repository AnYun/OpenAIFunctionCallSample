using Azure.AI.OpenAI;
using Azure;
using OpenAIFunctionCallSample.Models;
using System.Text.Json;

namespace OpenAIFunctionCallSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var apikey = "{your api key}";
            var apiUrl = "https://{your openai endpoint}.openai.azure.com";
            string deploymentName = "{your deployment name}";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.User, "我想要點兩份牛排")
                    //new ChatMessage(ChatRole.User, "請問台北市的天氣?")
                    //new ChatMessage(ChatRole.User, "請問 PS5 多少錢?")
                },
                Functions = FuntionDefinitions.GetFuntionDefinitions()
            };

            var client = new OpenAIClient(new Uri(apiUrl), new AzureKeyCredential(apikey));
            var response = await client.GetChatCompletionsAsync(
                deploymentOrModelName: deploymentName,
                chatCompletionsOptions);


            foreach (var choice in response.Value.Choices)
            {
                if (choice.FinishReason == CompletionsFinishReason.FunctionCall)
                {
                    switch (choice.Message.FunctionCall.Name)
                    {
                        case "CalFoodPrice":
                            var foodInfo = JsonSerializer.Deserialize<FoodInfo>(choice.Message.FunctionCall.Arguments);
                            var foodPrice = Functions.CalFoodPrice(foodInfo);
                            Console.WriteLine($"您點了 {foodInfo.Count} 份 {foodInfo.Food} 總共 {foodPrice} 元");
                            break;
                        case "GetCurrentWeather":
                            var weatherConfig = JsonSerializer.Deserialize<WeatherConfig>(choice.Message.FunctionCall.Arguments);
                            Console.WriteLine(Functions.GetCurrentWeather(weatherConfig));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.Write(choice.Message.Content);
                }
            }
        }
    }
}