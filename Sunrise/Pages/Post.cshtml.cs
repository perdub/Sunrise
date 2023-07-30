using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Types;

namespace Sunrise.Pages;

public class PostModel : PageModel
{
    public ContentType ContentType {get; set;}


    public string baseImageUrl {get;private set;}
    public string originalImageUrl {get;private set;}
    public string createTime{get;private set;}
    public string authorName {get;private set;}
    public string authorId{ get;private set;}
    public Tag[] tags {get;private set;}
    private readonly ILogger<PostModel> _logger;
    SunriseContext _context;
    public PostModel(ILogger<PostModel> logger, SunriseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync(Guid postid)
    {
        //todo: rewrite this shit
        var post = await _context.Posts.Include(a=>a.Tags).Where( b=> b.Id == postid).FirstOrDefaultAsync();
        
        var file = await _context.Files.FindAsync(post.FileId);
        ContentType = file.ContentType;
        baseImageUrl = file.Paths[1];
        originalImageUrl = file.Paths[2];
        createTime = post.PostCreationTime.ToString("o")+'Z';
        var author = await _context.Users.FindAsync(post.AuthorId);
        authorName = author?.Name;
        authorId = author.Id.ToString();
        tags = post.Tags.ToArray();
    }
}
