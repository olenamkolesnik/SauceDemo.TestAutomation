using Allure.NUnit.Attributes;
using Allure.NUnit;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using SauceDemo.TestAutomation.Config;
using SauceDemo.TestAutomation.Hooks;
using static System.Net.Mime.MediaTypeNames;

namespace SauceDemo.TestAutomation.Tests
{
    [AllureNUnit]
    [AllureSuite("Login")]
    [Category("Login")]
    public class LoginTests : PlaywrightTestBase
    {
        [Test]
        public async Task SuccessfulLogin_ShouldDisplayProductsPage()
        {
            Logger.LogInformation("Navigating to login page...");
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);

            Logger.LogInformation($"Logging in with {ConfigurationHelper.UserName}...");
            await _loginPage.Login(ConfigurationHelper.UserName, ConfigurationHelper.Password);

            Logger.LogInformation("Waiting for inventory list to be visible...");
            await Assertions.Expect(_productsPage.InventoryList).ToBeVisibleAsync();

            Logger.LogInformation("Verifying user is navigated to the products page...");
            var expectedUrl = $"{ConfigurationHelper.BaseUrl}/inventory.html";
            Assert.That(_productsPage.CurrentUrl, Is.EqualTo(expectedUrl), "User is not navigated to the products page after login.");
        }

        [Test]
        public async Task UnsuccessfulLogin_ShouldShowErrorMessage()
        {
            Logger.LogInformation("Navigating to login page...");
            await _loginPage.NavigateAsync(ConfigurationHelper.BaseUrl);

            Logger.LogInformation($"Logging in with {ConfigurationHelper.UserName} and incorrect password...");
            await _loginPage.Login(ConfigurationHelper.UserName, "IncorrectPassword");

            Logger.LogInformation("Waiting for error message to be visible...");
            var errorMessage = "Epic sadface: Username and password do not match any user in this service";
            await Assertions.Expect(_loginPage.ErrorMessage).ToContainTextAsync(errorMessage);
        }
    }
}
