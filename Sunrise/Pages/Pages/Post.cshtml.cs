using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Types;

namespace Sunrise.Pages;

public class PostModel : PageModel
{
    public string baseImageUrl {get;private set;}
    public string originalImageUrl {get;private set;}
    public string createTime{get;private set;}
    public string authorName {get;private set;}
    public string authorId{ get;private set;}
    public Tag[] tags {get;private set;}
    private readonly ILogger<PostModel> _logger;
    CacheService _cache;
    public PostModel(ILogger<PostModel> logger, CacheService cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task OnGetAsync(Guid postid)
    {
        //todo: rewrite this shit
        var post = await _cache.GetPostAsync(postid);
        var file = await _cache.GetFileAsync(post.FileId);
        baseImageUrl = file.Paths[1];
        originalImageUrl = file.Paths[2];
        createTime = post.PostCreationTime.ToString();
        var author = await _cache.GetUserAsync(post.AuthorId);
        authorName = author.Name;
        authorId = author.Id.ToString();
        tags = post.Tags.ToArray();
    }
}
