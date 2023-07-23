using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAIFunctionCallSample
{
    internal static class FuntionDefinitions
    {
        /// <summary>
        /// Get FuntionDefinitions
        /// </summary>
        /// <returns></returns>
        public static List<FunctionDefinition> GetFuntionDefinitions()
        {
            var calFoodPriceFuntionDefinition = new FunctionDefinition()
            {
                Name = "CalFoodPrice",
                Description = "計算客戶餐點價錢",
                Parameters = BinaryData.FromObjectAsJson(
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
                    }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };

            var getWeatherFuntionDefinition = new FunctionDefinition()
            {
                Name = "GetCurrentWeather",
                Description = "取得指定地點的天氣資訊",
                Parameters = BinaryData.FromObjectAsJson(
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
                    }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };

            return new List<FunctionDefinition> {
                calFoodPriceFuntionDefinition,
                getWeatherFuntionDefinition
            };
        }
    }
}
