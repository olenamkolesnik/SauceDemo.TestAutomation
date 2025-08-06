using System.Text.Json;
using SauceDemo.TestAutomation.Models;

namespace SauceDemo.TestAutomation.Helpers
{
    public static class TestDataHelper
    {
        public static List<Product> LoadProducts()
        {
            var json = File.ReadAllText("TestData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return products ?? [];
        }

    }
}