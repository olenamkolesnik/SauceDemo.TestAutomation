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
            var message = $"[{logLevel}] {_category}: {formatter(state, exception)}";
            NUnit.Framework.TestContext.Progress.WriteLine(message);
        }
    }

}
