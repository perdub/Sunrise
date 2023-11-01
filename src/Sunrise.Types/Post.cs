using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Sunrise.Types;

public class Post{
    [Key]
    public Guid PostId {get;private set;} = Guid.Empty;

    public Account PostCreator{get;private set;}

    public File LinkedFile{get;set;}

    public List<Tag> Tags{get;set;}

    public DateTime CreationDate{get;private set;}

    public string Description{get;set;}
    private Post(){}
    public Post(Account author)
    {
        PostCreator =  author;
        Description = "Your description";
        CreationDate = DateTime.UtcNow;
    }
}