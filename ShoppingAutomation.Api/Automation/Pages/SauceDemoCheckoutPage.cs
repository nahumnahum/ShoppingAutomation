using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Automation.Pages
{
    public class SauceDemoCheckoutPage : BasePage
    {
        // Selectors לשלבי ה-Checkout
        private ILocator CheckoutButton => Page.Locator("[data-test='checkout']");
        private ILocator FirstNameInput => Page.Locator("[data-test='firstName']");
        private ILocator LastNameInput  => Page.Locator("[data-test='lastName']");
        private ILocator ZipCodeInput   => Page.Locator("[data-test='postalCode']");
        private ILocator ContinueButton => Page.Locator("[data-test='continue']");
        private ILocator FinishButton   => Page.Locator("[data-test='finish']");
        private ILocator SuccessHeader  => Page.Locator("[data-test='complete-header']");

        public SauceDemoCheckoutPage(IPage page) : base(page) { }

        public async Task StartCheckout()
        {
            await HumanClick(CheckoutButton);
        }

        public async Task FillInformation(string firstName, string lastName, string zipCode)
        {
            await HumanFill(FirstNameInput, firstName);
            await HumanFill(LastNameInput, lastName);
            await HumanFill(ZipCodeInput, zipCode);
            await HumanClick(ContinueButton);
        }

        public async Task FinishOrder()
        {
            await HumanClick(FinishButton);
        }

        public async Task<bool> IsOrderComplete()
        {
            try 
            {
                await SuccessHeader.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}