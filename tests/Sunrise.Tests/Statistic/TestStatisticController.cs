namespace Sunrise.Tests;
//todo:rewrite this from ftba to resources
[AllureOwner("perdub")]
[AllureTag("Stats")]
public class StatisticTests
{
    [AllureXunit]
    public void CheckController()
    {
        var dbContext = this.GetDbContext();

        var controller = new Sunrise.Controllers.StatisticsController(dbContext);
        
        
        var r = controller.GetStatistic();

        
    }
    
}