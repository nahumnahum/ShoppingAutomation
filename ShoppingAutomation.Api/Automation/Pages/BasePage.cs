using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Automation.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage Page;
        protected readonly Random RandomDelay = new Random();

        protected BasePage(IPage page) => Page = page;

        protected async Task HumanClick(string selector) => await HumanClick(Page.Locator(selector).First);
        
        protected async Task HumanClick(ILocator locator)
        {
            await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 20000 });
            await locator.ScrollIntoViewIfNeededAsync();
            await Task.Delay(RandomDelay.Next(300, 600));
            await locator.ClickAsync();
        }

        protected async Task HumanFill(string selector, string text) => await HumanFill(Page.Locator(selector).First, text);

        protected async Task HumanFill(ILocator locator, string text)
        {
            await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 20000 });
            await locator.ClickAsync();
            await Page.Keyboard.TypeAsync(text, new() { Delay = RandomDelay.Next(50, 120) });
        }

        protected async Task ScrollAndHover(ILocator locator)
        {
            await locator.ScrollIntoViewIfNeededAsync();
            await locator.HoverAsync();
        }

        protected async Task RandomHumanPause(int minMs = 1000, int maxMs = 3000) 
        {
            await Task.Delay(RandomDelay.Next(minMs, maxMs));
        }

        protected async Task WaitForNetworkIdle() => await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        protected async Task TakeScreenshot(string path) => await Page.ScreenshotAsync(new() { Path = path, FullPage = true });

        protected async Task HandleCaptchaIfPresent(int waitSeconds = 20)
        {
            var captcha = Page.Locator("#captchacharacters, img[src*='captcha'], input[name='captcha_ps']");
            if (await captcha.CountAsync() > 0)
            {
                Console.WriteLine("⚠️ CAPTCHA Detected! Please solve it manually in the browser window...");
                await Task.Delay(waitSeconds * 1000);
            }
        }
    }
}