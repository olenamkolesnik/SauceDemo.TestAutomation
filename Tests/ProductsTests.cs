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
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);
            await _loginPage.Login(ConfigurationHelper.UserName, ConfigurationHelper.Password);
            await _productsPage.InventoryList.WaitForAsync();
        }

        [Test]
        public async Task ProductsShouldBeSortedAlphabeticallyDescendant()
        {
            await _productsPage.SortProductsBy("Name (Z to A)");
            var productNames = await _productsPage.GetAllProductNames();
            var sorted = productNames.OrderByDescending(d => d).ToList();
            Assert.That(productNames, Is.EqualTo(sorted), "Product names are not sorted in descending alphabetical order.");
        }
    }
}
