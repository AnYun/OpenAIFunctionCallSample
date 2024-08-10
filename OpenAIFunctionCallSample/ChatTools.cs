using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAIFunctionCallSample
{
    internal static class ChatTools
    {
        /// <summary>
        /// Get ChatTools
        /// </summary>
        /// <returns></returns>
        public static List<ChatTool> GetChatTools()
        {
            var calFoodPriceChatTool = ChatTool.CreateFunctionTool(
                functionName: "CalFoodPrice",
                functionDescription: "計算客戶餐點價錢",
                functionParameters: BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            Count = new
                            {
                                Type = "integer",
                                Description = "客戶點的餐點數量，比如說一份"
                            },
                            Food = new
                            {
                                Type = "string",
                                Enum = new[] { "牛排", "豬排" },
                                Description = "客戶點的餐點，比如說牛排或是豬排。"
                            }
                        },
                        Required = new[] { "count", "food" },
                    }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );

            var getWeatherChatTool = ChatTool.CreateFunctionTool(
                functionName: "GetCurrentWeather",
                functionDescription: "取得指定地點的天氣資訊",
                functionParameters: BinaryData.FromObjectAsJson(
                    new
                    {
                        Type = "object",
                        Properties = new
                        {
                            Location = new
                            {
                                Type = "string",
                                Description = "城市或鄉鎮地點",
                            },
                            Unit = new
                            {
                                Type = "string",
                                Enum = new[] { "攝氏", "華式" },
                            }
                        },
                        Required = new[] { "location" },
                    }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );

            return new List<ChatTool> {
                calFoodPriceChatTool,
                getWeatherChatTool
            };
        }
    }
}
