using Sunrise.Database;
using Sunrise.Types.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sunrise.Pages;

public class PostModel : SunModel
{
    private readonly ILogger<PostModel> _logger;
    private SunriseContext _context;

    public string baseUrl{get;private set;}
    public Sunrise.Types.Enums.PostType PostType{get;private set;}
    public string Description{get;private set;}
    public Types.Tag[] Tags{get; private set;}
    public string originalUrl{get;private set;}
    public string uploadTime{get;private set;}
    public string authorUsername{get;private set;}
    public Guid authorId{get;private set;}
    public PostRating Rating{get;private set;}

    public PostModel(ILogger<PostModel> logger, SunriseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> OnGet(Guid postid)
    {
        var post = _context.Posts
            .AsNoTracking()
            .Include(a=>a.LinkedFile)
            .Include(a=>a.Tags)
            .Include(a=>a.PostCreator)
            .Where(a=>a.PostId == postid)
            .FirstOrDefault();

        if(post is null){
            return NotFound();
        }

        baseUrl = post.LinkedFile.GetItemPath(ContentVariant.Sample).Replace("\\", "/");
        originalUrl = post.LinkedFile.GetItemPath(ContentVariant.Original).Replace("\\", "/");
        PostType = post.LinkedFile.PostType;
        Description = post.Description;
        Tags = post.Tags.ToArray();
        uploadTime = post.CreationDate.ToString();
        authorId = post.PostCreator.AccountId;
        authorUsername = post.PostCreator.Username;
        Rating = post.Rating;

        return Page();
    }
}