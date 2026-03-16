
namespace ShoppingAutomation.Api.Domain
{
   public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Currency { get; set; } = "USD";
        public string Url { get; set; } = string.Empty;
        public string Source { get; set; } = "Amazon";
    }
}