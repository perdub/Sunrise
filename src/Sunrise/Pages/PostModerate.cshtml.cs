namespace Sunrise.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Utilities;
using Sunrise.Types;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

public class PostModeratePage : SecurePageModel
{
    public string ItemUrl { get; private set; }
    public int FileType { get; private set; }
    public Types.Tag[] Tags { get; private set; }
    public Guid PostId { get; private set; }


    ILogger<PostModeratePage> _logger;

    public PostModeratePage(ILogger<PostModeratePage> logger, SunriseContext context) : base(context)
    {
        _logger = logger;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAllow())
        {
            return NotFound();
        }

        Post p = _context.Posts
            .Include(a => a.Tags)
            .Include(a => a.File)
            .OrderBy(a => a.PostCreationTime)
            .Where(a => (int)a.Status<Int32.MaxValue)
            .FirstOrDefault();

        //todo: add check for null

        var file = p.File;

        PostId = p.Id;
        Tags = p.Tags.ToArray();
        ItemUrl = file.Paths[1];
        FileType = (int)file.ContentType;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, string tags, int rating, int mode)
    {
        if (!IsAllow())
        {
            return NotFound();
        }

        switch (mode)
        {
            case 1:
                tags = Regex.Replace(tags, "[ \n]{1,}", " ").Trim();
                bool result = await _context.UpdatePostTag(id, tags);
                if (!result)
                {
                    _logger.LogWarning($"Fall to edit tags, post is null.", id);
                    return BadRequest("Fall to edit tags.");
                }
                var post = _context.Posts.Find(id);
                post.Rating = (Rating)rating;
                post.Status = PostStatus.ReadyToShow;
                _context.SaveChanges();
                break;
            case 2:

                break;
        }

        return Redirect("/postmoderate");
    }
}