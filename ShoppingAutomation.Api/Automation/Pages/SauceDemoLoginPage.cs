using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Automation.Pages
{
    public class SauceDemoLoginPage : BasePage
    {
        // Selectors יציבים המבוססים על data-test כנדרש בדרישות לאוטומציה איכותית
        private ILocator UsernameInput => Page.Locator("[data-test='username']");
        private ILocator PasswordInput => Page.Locator("[data-test='password']");
        private ILocator LoginButton   => Page.Locator("[data-test='login-button']");

        public SauceDemoLoginPage(IPage page) : base(page) { }

        public async Task Navigate()
        {
            // ניווט לאתר החדש
            await Page.GotoAsync("https://www.saucedemo.com/", new() { WaitUntil = WaitUntilState.NetworkIdle });
        }

        public async Task Login(string user, string pass)
        {
            await HumanFill(UsernameInput, user);
            await HumanFill(PasswordInput, pass);
            await HumanClick(LoginButton);
            // המתנה לטעינת דף המוצרים
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}