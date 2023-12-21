using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Sunrise.Types.Enums;

namespace Sunrise.Types;

public class Post{
    [Key]
    public Guid PostId {get;private set;} = Guid.Empty;

    public Account PostCreator{get;private set;}

    public File LinkedFile{get;set;}

    public List<Tag> Tags{get;set;} = new ();

    public DateTime CreationDate{get;private set;}

    public string Description{get;set;}
    public PostRating Rating{get;set;} = PostRating.Warning;
    private Post(){}
    public Post(Account author)
    {
        PostCreator =  author;
        Description = "Your description";
        CreationDate = DateTime.UtcNow;
    }
}