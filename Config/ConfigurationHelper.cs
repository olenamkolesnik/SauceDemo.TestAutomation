using System.Text.Json;

namespace SauceDemo.TestAutomation.Config
{
public static class ConfigurationHelper
    {
        private static readonly Dictionary<string, object> _config;

        static ConfigurationHelper()
        {
            DotNetEnv.Env.Load();

            var env = Environment.GetEnvironmentVariable("TEST_ENV") ?? "qa";
            var json = File.ReadAllText("appsettings.json");

            var fullConfig = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(json);
            if (fullConfig != null && fullConfig.TryGetValue(env, out var envConfig))
            {
                _config = envConfig;
            }
            else
            {
                throw new InvalidOperationException("Configuration for the specified environment is missing.");
            }
        }

        public static string BaseUrl => GetRequiredConfig("baseUrl");
        public static string Browser => GetRequiredConfig("browser");
        public static bool Headless => Convert.ToBoolean(GetRequiredConfig("headless"));
        public static int Timeout => Convert.ToInt32(GetRequiredConfig("timeout"));
        public static string UserName => GetEnvironmentVariable("USERNAME");
        public static string Password => GetEnvironmentVariable("USERPASSWORD");

        private static string GetRequiredConfig(string key)
        {
            if (_config.TryGetValue(key, out var value) && value != null)
            {
                return value.ToString() ?? throw new InvalidOperationException($"Configuration value for key '{key}' is null.");
            }
            throw new InvalidOperationException($"Configuration key '{key}' is missing or has no value.");
        }

        private static string GetEnvironmentVariable(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Environment variable '{key}' is not set.");
            }
            return value;
        }
    }
}
