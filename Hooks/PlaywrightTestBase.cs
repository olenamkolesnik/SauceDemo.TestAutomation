using Allure.Commons;
using Allure.NUnit;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
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

            if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var screenshotPath = Path.Combine("TestResults", $"{testName}.png");

                // Ensure directory exists  
                Directory.CreateDirectory("TestResults");

                await _testContext.Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = screenshotPath,
                    FullPage = true
                });

                Logger.LogInformation("Screenshot saved at path: {ScreenshotPath}", screenshotPath);

                // Attach screenshot to Allure report  
                var lifecycle = AllureLifecycle.Instance;
                lifecycle.AddAttachment(testName, "image/png", screenshotPath);
            }

            await _testContext.Page.CloseAsync();
            await _testContext.Context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
