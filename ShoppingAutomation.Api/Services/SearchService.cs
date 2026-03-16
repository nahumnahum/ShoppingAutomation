using ShoppingAutomation.Api.Automation;

namespace ShoppingAutomation.Api.Services
{
    public class SearchService
    {
        private readonly ShopAutomation _automation;

        public SearchService(ShopAutomation automation)
        {
            _automation = automation;
        }

        public async Task<AutomationResult> RunFullAutomation(
            string query,
            string email,
            string password,
            string? maxPrice = null)
        {
            return await _automation.RunAutomation(email, password, query, maxPrice);
        }
    }
}