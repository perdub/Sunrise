using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunrise.Database;
using Sunrise.Types;
using Microsoft.Extensions.DependencyInjection;
using Sunrise.Controllers;
using Microsoft.Extensions.Logging;

namespace Sunrise.Pages;

public class ListModel : SunModel{
    //60 because is divided by 1, 2, 3 and 5
    const int POST_PER_PAGE = 60;

    public Post[] Posts{get;set;}
    public string? nextPageUrl{get;set;} = null;
    public string? prevPageUrl{get;set;} = null;

    private SunriseContext _context;
    private IServiceProvider _provider;

    public ListModel(SunriseContext context, IServiceProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public async Task<IActionResult> OnGet(
        [FromQuery] int pageNumber = 1,
        [FromQuery] bool randomOrder = false
        ){
        pageNumber--;

        if(pageNumber > 0){
            prevPageUrl = "/list?pageNumber=" + (pageNumber);
        }
        //todo: add statement here
        if(true){
            nextPageUrl = "/list?pageNumber=" + (pageNumber + 2);
        }
            
        //build controller
        var sunriseContext = _provider.GetRequiredService<SunriseContext>();
        var logger = _provider.GetRequiredService<ILoggerFactory>().CreateLogger<FindController>();
        var findController = FindController.GetController(sunriseContext, logger);

        Posts = await findController.FindPosts(null, pageNumber * POST_PER_PAGE, POST_PER_PAGE, randomOrder);
        
        return Page();
    }
}