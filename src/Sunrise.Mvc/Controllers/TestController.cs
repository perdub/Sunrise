using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Mvc;

public class TestController : Controller{
    [HttpGet]
    [Route("/test")]
    public IActionResult Test(){
        return Ok();
    }
}