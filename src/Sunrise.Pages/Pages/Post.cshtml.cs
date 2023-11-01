using Sunrise.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sunrise.Pages;

public class PostModel : PageModel
{
    private readonly ILogger<PostModel> _logger;
    private SunriseContext _context;

    public string baseUrl{get;private set;}
    public Sunrise.Types.Enums.PostType PostType{get;private set;}

    public PostModel(ILogger<PostModel> logger, SunriseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> OnGet(Guid postid)
    {
        var post = _context.Posts
            .Include(a=>a.LinkedFile)
            .Include(a=>a.Tags)
            .Include(a=>a.PostCreator)
            .Where(a=>a.PostId == postid)
            .FirstOrDefault();

        if(post is null){
            return NotFound();
        }

        baseUrl = post.LinkedFile.GetBaseLink();
        PostType = post.LinkedFile.PostType;

        return Page();
    }
}