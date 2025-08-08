using Allure.Commons;
using Allure.Net.Commons;
using Allure.NUnit;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using NUnit.Framework.Interfaces;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Helpers;
using SauceDemo.TestAutomation.Pages;

namespace SauceDemo.TestAutomation.Hooks
{
    [AllureNUnit]
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightTestBase
    {
        protected Context.TestContext _testContext;
        protected IPlaywright _playwright;
        protected IBrowser _browser;
        protected LoginPage _loginPage;
        protected ProductsPage _productsPage;

        protected ILogger Logger { get; private set; }

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Information)
                    .AddConsole()
                    .AddProvider(new TestContextLoggerProvider());
            });

            Logger = loggerFactory.CreateLogger("PlaywrightTests");
        }

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();

            var browserType = ConfigurationHelper.Browser.ToLower() switch
            {
                "firefox" => _playwright.Firefox,
                "webkit" => _playwright.Webkit,
                _ => _playwright.Chromium
            };

            _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = ConfigurationHelper.Headless
            });

            var browserContext = await _browser.NewContextAsync();
            var page = await browserContext.NewPageAsync();
            page.SetDefaultTimeout(ConfigurationHelper.Timeout);

            _testContext = new Context.TestContext()
            {
                Context = browserContext,
                Page = page
            };

            _loginPage = new LoginPage(_testContext.Page);
            _productsPage = new ProductsPage(_testContext.Page);
        }

        [TearDown]
        public async Task TearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            if (outcome == TestStatus.Failed)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var baseDir = AppContext.BaseDirectory;
                var screenshotsDir = Path.Combine(baseDir, "screenshots");
                Directory.CreateDirectory(screenshotsDir);

                var fileName = $"{testName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png";
                var screenshotPath = Path.Combine(screenshotsDir, fileName);

                // Capture screenshot and write to disk (Playwright returns bytes)
                var bytes = await _testContext.Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = screenshotPath,
                    FullPage = true
                });

                // Defensive: if Playwright didn't return bytes for some reason, read file
                if (bytes == null || bytes.Length == 0)
                {
                    bytes = File.ReadAllBytes(screenshotPath);
                }

                // Attach screenshot bytes to Allure
                AllureApi.AddAttachment("Failure Screenshot", "image/png", bytes, "png");

                Logger.LogInformation($"Screenshot saved at: {screenshotPath}");
            }

            await _testContext.Page.CloseAsync();
            await _testContext.Context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
