using Sunrise.Database;
using Sunrise.Types;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Builders;

public class TagBuilder{
    private SunriseContext _context;

    public TagBuilder(SunriseContext context)
    {
        _context = context;
    }

    public async Task<Tag[]> GetTags(params string[] tags){
        var dbTags = await _context.Tags
            .Where(a => tags.Contains(a.TagText))
            .ToListAsync();

        var newTags = tags.Except(dbTags.Select(a => a.TagText)).ToArray();

        foreach(var tag in newTags){
            Tag newTag = new Tag(tag);
            _context.Tags.Add(newTag);
            dbTags.Add(newTag);
        }

        await _context.SaveChangesAsync();

        return dbTags.ToArray();
    }

}