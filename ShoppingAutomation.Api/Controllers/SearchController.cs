using ShoppingAutomation.Api.Services;
using ShoppingAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ShoppingAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchService   _service;
    private readonly IConfiguration  _config;

    public SearchController(SearchService service, IConfiguration config)
    {
        _service = service;
        _config  = config;
    }

    [HttpPost("run")]
    public async Task<IActionResult> RunAutomation([FromBody] SearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return BadRequest(new { error = "Query is required." });

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { error = "Email and Password are required." });

        var result = await _service.RunFullAutomation(
            request.Query,
            request.Email,
            request.Password,
            request.MaxPrice);

        // כאן ה-Result כבר כולל את שני נתיבי התמונות
        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    [HttpPost("run-default")]
    public async Task<IActionResult> RunWithDefaults()
    {
        var email    = _config["Automation:Email"];
        var password = _config["Automation:Password"];
        var query    = _config["Automation:DefaultQuery"] ?? "laptop";
        var maxPrice = _config["Automation:MaxPrice"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return BadRequest(new { error = "Email/Password not configured in appsettings.json" });

        var result = await _service.RunFullAutomation(query!, email!, password!, maxPrice);
        return result.Success ? Ok(result) : StatusCode(500, result);
    }
}