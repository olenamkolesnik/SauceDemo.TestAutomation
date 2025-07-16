using Microsoft.Playwright;

namespace SauceDemo.TestAutomation.Context
{
    public class TestContext
    {
        public required IPage Page { get; set; }
        public required IBrowserContext Context { get; set; }

    }
}