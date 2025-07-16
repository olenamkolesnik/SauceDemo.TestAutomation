using Microsoft.Extensions.Logging;

namespace SauceDemo.TestAutomation.Helpers
{
    public class TestContextLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new TestContextLogger(categoryName);
        }

        public void Dispose() { }
    }

    public class TestContextLogger : ILogger
    {
        private readonly string _category;

        public TestContextLogger(string category)
        {
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state) => null!;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
    TState state, Exception exception, Func<TState, Exception?, string> formatter)
        {
            var testName = TestContext.CurrentContext.Test.Name ?? "UnknownTest";
            var message = formatter(state, exception);

            var formattedMessage = $"[{logLevel}] {_category} [{testName}]: {message}";

            switch (logLevel)
            {
                case LogLevel.Error:
                    Console.WriteLine($"::error::{formattedMessage}");
                    break;
                case LogLevel.Warning:
                    Console.WriteLine($"::warning::{formattedMessage}");
                    break;
                case LogLevel.Information:
                    Console.WriteLine($"::notice::{formattedMessage}");
                    break;
            }
        }
    }

}
