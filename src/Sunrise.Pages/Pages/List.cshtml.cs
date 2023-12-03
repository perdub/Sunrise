using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunrise.Database;
using Sunrise.Types;
using Microsoft.Extensions.DependencyInjection;
using Sunrise.Controllers;
using Microsoft.Extensions.Logging;

namespace Sunrise.Pages;

public class ListModel : SunModel{
    const int POST_PER_PAGE = 50;

    public Post[] Posts{get;set;}

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
            
        //build controller
        var sunriseContext = _provider.GetRequiredService<SunriseContext>();
        var logger = _provider.GetRequiredService<ILoggerFactory>().CreateLogger<FindController>();
        var findController = FindController.GetController(sunriseContext, logger);

        Posts = await findController.FindPosts(null, pageNumber * POST_PER_PAGE, POST_PER_PAGE, randomOrder);
        
        return Page();
    }
}