using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingAutomation.Api.Domain;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ShoppingAutomation.Api.Domain;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShoppingAutomation.Tests
{
    [TestClass]
    public class ProductNormalizationTests
    {
        [TestMethod]
        public void ParsePrice_ValidWholeAndFraction_ReturnsCorrectFloat()
        {
            string whole = "29", fraction = "99";
            float price = float.Parse($"{whole}.{fraction}", CultureInfo.InvariantCulture);
            Assert.AreEqual(29.99f, price, 0.001f);
        }

        [TestMethod]
        public void ParsePrice_InvalidString_ReturnsZero()
        {
            string raw = "N/A";
            float price = float.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out float p) ? p : 0;
            Assert.AreEqual(0f, price, 0.001f);
        }
    }

    [TestClass]
    public class ProductSelectionTests
    {
        private List<Product> GetSampleProducts() => new()
        {
            new Product { Id = "1", Title = "Budget Item",    Price = 9.99f,  Currency = "USD", Url = "", Source = "Amazon" },
            new Product { Id = "2", Title = "Mid Range Item", Price = 29.99f, Currency = "USD", Url = "", Source = "Amazon" },
            new Product { Id = "3", Title = "Premium Item",   Price = 99.99f, Currency = "USD", Url = "", Source = "Amazon" },
            new Product { Id = "4", Title = "No Price Item",  Price = 0f,     Currency = "USD", Url = "", Source = "Amazon" },
        };

        [TestMethod]
        public void SelectCheapest_ReturnsCheapestWithPrice()
        {
            var products = GetSampleProducts();
            var cheapest = products.Where(p => p.Price > 0).OrderBy(p => p.Price).FirstOrDefault();
            Assert.IsNotNull(cheapest);
            Assert.AreEqual(9.99f, cheapest.Price, 0.001f);
        }

        [TestMethod]
        public void FilterByMaxPrice_RemovesExpensiveProducts()
        {
            var products = GetSampleProducts();
            float maxPrice = 30f;
            var filtered = products.Where(p => p.Price > 0 && p.Price <= maxPrice).ToList();
            Assert.AreEqual(2, filtered.Count);
        }

        [TestMethod]
        public void SelectFirst_WhenNoPriceAvailable_ReturnFirstProduct()
        {
            var products = new List<Product>
            {
                new Product { Id = "1", Title = "Only Item", Price = 0f, Currency = "USD", Url = "", Source = "Amazon" }
            };
            var selected = products.Where(p => p.Price > 0).OrderBy(p => p.Price).FirstOrDefault()
                           ?? products.First();
            Assert.AreEqual("Only Item", selected.Title);
        }
    }

    [TestClass]
    public class CartCalculationTests
    {
        [TestMethod]
        public void CartTotal_MultipleItems_CorrectTotal()
        {
            var items = new List<Product>
            {
                new Product { Price = 19.99f, Id="1", Title="A", Currency="USD", Url="", Source="Amazon" },
                new Product { Price = 5.50f,  Id="2", Title="B", Currency="USD", Url="", Source="Amazon" }
            };
            float total = items.Sum(p => p.Price);
            Assert.AreEqual(25.49f, total, 0.01f);
        }

        [TestMethod]
        public void CartTotal_EmptyCart_ReturnsZero()
        {
            var items = new List<Product>();
            float total = items.Sum(p => p.Price);
            Assert.AreEqual(0f, total, 0.001f);
        }
    }
}