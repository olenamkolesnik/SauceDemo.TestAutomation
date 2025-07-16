using Microsoft.Playwright;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Pages;

namespace SauceDemo.TestAutomation.Hooks
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightTestBase
    {
        protected Context.TestContext _testContext;
        protected IPlaywright _playwright;
        protected IBrowser _browser;

        protected LoginPage _loginPage;
        protected ProductsPage _productsPage;

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
            await _testContext.Page.CloseAsync();
            await _testContext.Context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
