using Microsoft.Playwright;
using ShoppingAutomation.Api.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Automation.Pages
{
    public class SauceDemoInventoryPage : BasePage
    {
        // סלקטור לכל כרטיס מוצר ברשימה
        private ILocator InventoryItems => Page.Locator("[data-test='inventory-item']");

        public SauceDemoInventoryPage(IPage page) : base(page) { }

        /// <summary>
        /// אוסף מוצרים מהדף ומסנן אותם לפי שאילתת החיפוש של המשתמש.
        /// זה מממש את דרישת ה"חיפוש" (סעיף 3.1) באתר ללא תיבת חיפוש.
        /// </summary>
        public async Task<List<Product>> GetProductsAsync(string query)
        {
            var products = new List<Product>();
            
            // המתנה שהאלמנטים יופיעו בדף (Explicit Wait)
            await InventoryItems.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            
            var items = await InventoryItems.AllAsync();

            foreach (var item in items)
            {
                var titleElement = item.Locator("[data-test='inventory-item-name']");
                string title = await titleElement.InnerTextAsync();

                // מימוש ה"חיפוש": בדיקה אם שם המוצר מכיל את מילת החיפוש
                if (string.IsNullOrEmpty(query) || title.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    var priceText = await item.Locator("[data-test='inventory-item-price']").InnerTextAsync();
                    
                    // נורמליזציה של המחיר (סעיף 5 בדרישות): הסרת סימן $ והמרה ל-float
                    float price = float.Parse(priceText.Replace("$", ""), CultureInfo.InvariantCulture);

                    products.Add(new Product
                    {
                        Id = title.Replace(" ", "-").ToLower(),
                        Title = title,
                        Price = price,
                        Source = "SauceDemo",
                        Url = Page.Url // ב-SauceDemo הקטלוג כולו באותו URL
                    });
                }
            }
            return products;
        }

        /// <summary>
        /// מוסיף מוצר ספציפי לעגלה לפי השם שלו
        /// </summary>
        public async Task AddToCart(string productName)
        {
            // SauceDemo משתמש בפורמט קבב (kebab-case) לסלקטורים של כפתורי ההוספה
            var kebabName = productName.ToLower().Replace(" ", "-");
            var addToCartBtn = Page.Locator($"[data-test='add-to-cart-{kebabName}']");
            
            await HumanClick(addToCartBtn);
        }

        public async Task GoToCart()
        {
            await HumanClick("[data-test='shopping-cart-link']");
        }
    }
}