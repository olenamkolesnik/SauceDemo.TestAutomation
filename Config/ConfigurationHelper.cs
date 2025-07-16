using Microsoft.Extensions.Configuration;

namespace SauceDemo.TestAutomation.Config
{
    public static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _configuration;

        static ConfigurationHelper()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string BaseUrl => GetRequiredConfig("BaseUrl");
        public static string Browser => GetRequiredConfig("Browser"); 
        public static bool Headless => bool.Parse(GetRequiredConfig("Headless"));
        public static int Timeout => int.Parse(GetRequiredConfig("Timeout"));
        public static string UserName => GetRequiredConfig("UserName");
        public static string Password => GetRequiredConfig("Password");

        private static string GetRequiredConfig(string key)
        {
            var value = _configuration[key];
            return value is null ? throw new InvalidOperationException($"Missing required configuration: {key}") : value;
        }
    }
}
