using Microsoft.AspNetCore.Mvc;
using Sunrise.Types;

namespace Sunrise.Controllers;

public class StatisticsController : Controller{
    SunriseContext sc;

    public StatisticsController(SunriseContext sunriseContext)
    {
        sc = sunriseContext;
    }
    
    [Route("/stats")]
    public IActionResult GetStatistic(){
        ServerStats s = new ServerStats{
            posts = sc.Posts.Count(),
            tags = sc.Tags.Count(),
            users = sc.Users.Count(),
            uptime = DateTime.UtcNow - Program.StartTime,
            version = $"{Program.VERSION}-{Program.CONFIG}"
        };
        string result = Newtonsoft.Json.JsonConvert.SerializeObject(s);
        return Ok(result);
    }
}