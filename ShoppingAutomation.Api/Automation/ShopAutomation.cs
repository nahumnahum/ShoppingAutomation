using Microsoft.Playwright;
using ShoppingAutomation.Api.Automation.Pages;
using ShoppingAutomation.Api.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Automation
{
    public class ShopAutomation
    {
        private readonly string _screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "screenshots");

        public ShopAutomation()
        {
            // יצירת תיקיית צילומי המסך אם היא לא קיימת
            if (!Directory.Exists(_screenshotDir)) Directory.CreateDirectory(_screenshotDir);
        }

        public async Task<AutomationResult> RunAutomation(string email, string password, string query, string? maxPriceFilter = null)
        {
            var logger = new AutomationLogger();
            var result = new AutomationResult { RequestId = logger.RequestId };

            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // משאירים false כדי לראות את הדפדפן רץ
                Args = new[] { "--disable-blink-features=AutomationControlled" }
            });

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
            });

            var page = await context.NewPageAsync();

            try
            {
                // 1. ניווט לאתר
                logger.StartStep("Navigate to SauceDemo");
                var loginPage = new SauceDemoLoginPage(page);
                await loginPage.Navigate();
                logger.EndStep();

                // 2. ביצוע התחברות
                logger.StartStep("Login");
                await loginPage.Login(email, password);
                logger.EndStep();

                // 3. חיפוש וסינון מוצרים
                logger.StartStep($"Search and Filter: '{query}'");
                var inventoryPage = new SauceDemoInventoryPage(page);
                var allProducts = await inventoryPage.GetProductsAsync(query);

                if (float.TryParse(maxPriceFilter, out float maxPrice) && maxPrice > 0)
                {
                    allProducts = allProducts.Where(p => p.Price <= maxPrice).ToList();
                }

                result.Products = allProducts;
                logger.EndStep(allProducts.Count > 0);

                if (!allProducts.Any()) 
                    throw new Exception($"No products found matching '{query}' under price {maxPriceFilter}");

                // 4. בחירת המוצר הזול ביותר מהרשימה
                var cheapest = allProducts.OrderBy(p => p.Price).First();
                result.SelectedProduct = cheapest;
                logger.LogInfo($"Selected cheapest product: {cheapest.Title} (${cheapest.Price})");

                // 5. הוספה לסל ומעבר לעגלה
                logger.StartStep("Add to cart and Preview");
                await inventoryPage.AddToCart(cheapest.Title);
                await inventoryPage.GoToCart();
                
                // --- צילום מסך 1: הוכחת בחירת מוצר (סל הקניות) ---
                string summaryFileName = $"summary_{logger.RequestId}.png";
                string summaryPath = Path.Combine(_screenshotDir, summaryFileName);
                await page.ScreenshotAsync(new() { Path = summaryPath, FullPage = true });
                result.SummaryScreenshotPath = $"/screenshots/{summaryFileName}";
                logger.LogInfo("Captured order summary screenshot.");
                logger.EndStep();

                // 6. תהליך התשלום (Checkout)
                logger.StartStep("Checkout process");
                var checkoutPage = new SauceDemoCheckoutPage(page);
                await checkoutPage.StartCheckout();
                
                // מילוי פרטים פיקטיביים
                await checkoutPage.FillInformation("Automation", "User", "12345");
                await checkoutPage.FinishOrder();
                
                // 7. בדיקת הצלחה וצילום סופי
                bool isSuccess = await checkoutPage.IsOrderComplete();
                
                // --- צילום מסך 2: אישור הזמנה סופי ---
                string successFileName = $"success_{logger.RequestId}.png";
                string proofPath = Path.Combine(_screenshotDir, successFileName);
                await page.ScreenshotAsync(new() { Path = proofPath, FullPage = true });
                
                result.ScreenshotPath = $"/screenshots/{successFileName}";
                result.Success = isSuccess;
                logger.EndStep(isSuccess);
            }
            catch (Exception ex)
            {
                logger.EndStep(false, ex.Message);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                await browser.CloseAsync();
            }

            result.Steps = logger.Steps;
            return result;
        }
    }
}