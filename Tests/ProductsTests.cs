using Allure.NUnit.Attributes;
using Allure.NUnit;
using Microsoft.Extensions.Logging;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Hooks;

namespace SauceDemo.TestAutomation.Tests
{
    [Category("Products")]
    public class ProductsTests : PlaywrightTestBase
    {
        [SetUp]
        public async Task LoginBeforeEachTest()
        {
            Logger.LogInformation("Navigating to login page...");
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);

            Logger.LogInformation($"Logging in with {ConfigurationHelper.UserName}...");
            await _loginPage.Login(ConfigurationHelper.UserName, ConfigurationHelper.Password);

            Logger.LogInformation("Waiting for inventory list to be visible...");
            await _productsPage.InventoryList.WaitForAsync();
        }

        [AllureSuite("Products")]
        [Test]
        public async Task ProductsShouldBeSortedAlphabeticallyDescendant()
        {
            Logger.LogInformation("Sorting products by name in descending order...");
            await _productsPage.SortProductsBy("Name (Z to A)");

            Logger.LogInformation("Verifying product names are sorted in descending alphabetical order...");
            var productNames = await _productsPage.GetAllProductNames();
            var sorted = productNames.OrderByDescending(d => d).ToList();
            Assert.That(productNames, Is.EqualTo(sorted), "Product names are not sorted in descending alphabetical order.");
        }
    }
}
