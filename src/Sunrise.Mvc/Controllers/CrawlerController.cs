using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Mvc;

public class CrawlerController : Controller
{
    private const string ROBOTS_TXT = """
    User-agent: *
    Disallow: /forms/upload
    Disallow: /forms/login
    Disallow: /forms/registry
    Disallow: /upload
    Disallow: /api/upload
    """;

    [Route("/robots.txt")]
    public async Task<IActionResult> Robots(){
        return Ok(ROBOTS_TXT);
    }
}