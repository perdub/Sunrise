using Sunrise.Database;
using Sunrise.Types.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sunrise.Types;
using Sunrise.Builders;

namespace Sunrise.Pages;

[IgnoreAntiforgeryToken]
public class ReviewModel : SecureModel
{
    public Post? Post{get;private set;}
    public bool RandomTake {get;set;}

    private SunriseContext _context;
    private TagBuilder _tagBuilder;
    public ReviewModel(SunriseContext context) : base(context) 
    { 
        _context = context; 
        //create tag builder here because in this way he will be have same SunriseContext
        //yes i know that is bad but i make all request to context syncronously(im hope)
        _tagBuilder = new (_context);
    }

    public async Task<IActionResult> OnGet(Guid? postid, bool randomTake = false){
        if(!Allow(PrivilegeLevel.Moderator)){
            return NotFound();
        }

        this.RandomTake = randomTake;

        var postQ = _context.Posts
            .AsNoTracking()
            .Include(a=>a.LinkedFile)
            .Include(a=>a.Tags)
            .Include(a=>a.PostCreator);

        if(postid is not null){
            Post = postQ
                .Where(a=>a.PostId == postid)
                .FirstOrDefault();
        }
        else{
            if(randomTake){
                Post = postQ
                    .OrderBy(a => EF.Functions.Random())
                    .FirstOrDefault();
            }
            else{
                Post = postQ
                    .OrderByDescending(a => a.CreationDate)
                    .FirstOrDefault();
            }
        }

        

        return Page();
    }

    public async Task<IActionResult> OnPost(
        [FromForm] Guid id,
        [FromForm] string tags,
        [FromForm] int rating,
        [FromForm] string description,

        [FromQuery] bool randomTake
    ){
        if(!Allow(PrivilegeLevel.Moderator)){
            return NotFound();
        }

        //validation
        if(tags is null){
            tags = string.Empty;
        }
        if(description is null){
            description = string.Empty;
        }

        var post = _context.Posts
            .Include(a=>a.Tags)
            .Where(a=>a.PostId == id)
            .FirstOrDefault();

        if(post is null){
            return NotFound();
        }

        post.Rating = (PostRating)rating;
        post.Description = description;

        var t = tags.Split(" ");
        

        foreach(var tg in post.Tags){
            tg.PostCount--;
            tg.Posts.Remove(post);
        }

        var newTags = await _tagBuilder.GetTags(t);
        // todo: dont delete all tags and add it again
        post.Tags = new ();

        foreach(var tag in newTags){
            post.Tags.Add(tag);
            tag.PostCount++;
            tag.Posts.Add(post);
        }

        _context.SaveChanges();

        return Redirect("/review?randomTake=" + randomTake);
    }
}