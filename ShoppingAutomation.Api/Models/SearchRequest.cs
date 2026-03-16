namespace ShoppingAutomation.Api.Models
{
    public class SearchRequest
    {
        public string Query    { get; set; } = null!;
        public string Email    { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? MaxPrice { get; set; }
    }
}