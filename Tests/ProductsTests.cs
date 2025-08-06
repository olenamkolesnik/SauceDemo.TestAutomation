using Allure.NUnit.Attributes;
using Microsoft.Extensions.Logging;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Helpers;
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

        [AllureSuite("Products")]
        [Test]
        public async Task ProductsShouldMatchExpectedDataFromJson()
        {
            var expectedProducts = TestDataHelper.LoadProducts();          

            Logger.LogInformation("Fetching all product details from UI...");
            var actualProducts = await _productsPage.GetAllProductDetails();

            Logger.LogInformation("Validating that expected products are present in UI...");
            foreach (var expected in expectedProducts)
            {
                var actual = actualProducts.FirstOrDefault(p => p.Name == expected.Name);
                Assert.That(actual, Is.Not.Null, $"Expected product '{expected.Name}' not found on UI.");
                Assert.Multiple(() =>
                {
                    Assert.That(actual.Name, Is.EqualTo(expected.Name), $"Name mismatch for '{expected.Name}'");
                    Assert.That(actual.Price, Is.EqualTo(expected.Price), $"Price mismatch for '{expected.Name}'");
                    Assert.That(actual.Description, Is.EqualTo(expected.Description), $"Description mismatch for '{expected.Name}'");
                });
            }
        }

    }
}
