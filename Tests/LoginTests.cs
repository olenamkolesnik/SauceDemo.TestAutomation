using Microsoft.Playwright;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Hooks;
using static System.Net.Mime.MediaTypeNames;

namespace SauceDemo.TestAutomation.Tests
{
    [Category("Login")]
    public class LoginTests : PlaywrightTestBase
    {
        [Test]
        public async Task SuccessfulLogin_ShouldDisplayProductsPage()
        {
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);
            await _loginPage.Login(ConfigurationHelper.UserName, ConfigurationHelper.Password);

            await Assertions.Expect(_productsPage.InventoryList).ToBeVisibleAsync();
            var expectedUrl = $"{ConfigurationHelper.BaseUrl}/inventory.html";
            Assert.That(_productsPage.CurrentUrl, Is.EqualTo(expectedUrl), "User is not navigated to the products page after login.");
        }

        [Test]
        public async Task UnsuccessfulLogin_ShouldShowErrorMessage()
        {
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);
            await _loginPage.Login(ConfigurationHelper.UserName, "IncorrectPassword");

            var errorMessage = "Epic sadface: Username and password do not match any user in this service";
            await Assertions.Expect(_loginPage.ErrorMessage).ToContainTextAsync(errorMessage);
        }
    }
}
