using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Tests;
//todo:rewrite this from ftba to resources
[AllureOwner("perdub")]
[AllureTag("Stats")]
public class StatisticTests
{
    [AllureXunit]
    public void CheckController()
    {
        var dbContext = this.GetDbContext().AddTestData();
        var controller = new Sunrise.Controllers.StatisticsController(dbContext);
        
        var r = (ObjectResult)controller.GetStatistic();

        Assert.NotNull(r);
        var st = (Sunrise.Types.ServerStats)r.Value;
        Assert.NotNull(st);
    }
    
}