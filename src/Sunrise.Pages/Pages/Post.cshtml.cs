using Sunrise.Database;
using Sunrise.Types.Enums;
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
    public string Description{get;private set;}
    public Types.Tag[] Tags{get; private set;}
    public string originalUrl{get;private set;}

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

        baseUrl = post.LinkedFile.GetItemPath(ContentVariant.Sample);
        originalUrl = post.LinkedFile.GetItemPath(ContentVariant.Original);
        PostType = post.LinkedFile.PostType;
        Description = post.Description;
        Tags = post.Tags.ToArray();

        return Page();
    }
}