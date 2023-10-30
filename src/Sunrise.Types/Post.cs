using System.ComponentModel.DataAnnotations;

namespace Sunrise.Types;

public class Post{
    [Key]
    public Guid PostId {get;private set;} = Guid.Empty;

    public Account PostCreator{get;private set;}

    public File LinkedFile{get;set;}

    public Tag[] Tags{get;set;}

    public DateTime CreationDate{get;private set;}

    public string Description{get;set;}

}