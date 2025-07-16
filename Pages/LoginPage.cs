using Microsoft.Playwright;

namespace SauceDemo.TestAutomation.Pages
{
    public class LoginPage(IPage page)
    {
        private readonly IPage _page = page;

        public ILocator ErrorMessage => _page.Locator("[data-test=\"error\"]");
        private ILocator LoginButton => _page.Locator("#login-button");
        private ILocator UsernameInput => _page.Locator("#user-name");
        private ILocator PasswordInput => _page.Locator("#password");

        public async Task NavigateAsync(string url)
        {
            await _page.GotoAsync(url);
            await LoginButton.WaitForAsync();
        }

        public async Task Login(string username, string password)
        {
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await LoginButton.ClickAsync();
        }
    }
}
