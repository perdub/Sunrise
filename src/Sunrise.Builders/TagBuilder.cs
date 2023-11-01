using Sunrise.Database;
using Sunrise.Types;

namespace Sunrise.Builders;

public class TagBuilder{
    private SunriseContext _context;

    public TagBuilder(SunriseContext context)
    {
        _context = context;
    }

    public async Task<Tag[]> GetTags(params string[] tags){
        List<Tag> rTag = new List<Tag>(tags.Length);
        //todo: может быть возможно получить все теги из бд одним запросом. надо будет попробывать потом.
        foreach(var tag in tags){
            var sTag = _context.Tags.Where(a=>a.TagText==tag).FirstOrDefault();
            if(sTag is null){
                sTag = new Tag(tag);
                _context.Tags.Add(sTag);
            }
            rTag.Add(sTag);
        }
        await _context.SaveChangesAsync();
        return rTag.ToArray();
    }

}