using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Controllers;

public class StatisticsController : Controller{
    SunriseContext sc;

    public StatisticsController(SunriseContext sunriseContext)
    {
        sc = sunriseContext;
    }
    
    [Route("/stats")]
    public IActionResult GetStatistic(){
        return Ok(new {
            post = sc.Posts.Count(),
            tags = sc.Tags.Count(),
            users = sc.Users.Count(),
            uptime = (DateTime.UtcNow-Program.StartTime)
        });
    }
}