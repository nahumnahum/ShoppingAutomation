using ShoppingAutomation.Api.Domain;
using System.Collections.Generic;

namespace ShoppingAutomation.Api.Automation
{
    public class AutomationResult
    {
        public string RequestId         { get; set; } = "";
        public bool   Success           { get; set; }
        public string? ErrorMessage     { get; set; }
        public string? ScreenshotPath   { get; set; }
        
        // השדה החדש שהוספנו כדי שהפרונטאנד יוכל להציג את תמונת הסל
        public string? SummaryScreenshotPath { get; set; }
        
        public List<Product> Products   { get; set; } = new();
        public Product? SelectedProduct { get; set; }
        public List<StepLog> Steps      { get; set; } = new();
    }
}