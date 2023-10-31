namespace Sunrise.Types;

using System.ComponentModel.DataAnnotations;
using Sunrise.Types.Enums;

public class File{
    public PostType PostType {get;init;} = Enums.PostType.Unknown;

    public string previewPath {get;init;}
    public string samplePath {get;init;}
    public bool isSampleExsist {get;init;} = false;
    public string fullPath {get;init;}

    public Post LinkedPost{get;private set;}
    public Guid PostId{get;private set;} = Guid.Empty;

    [Key]
    public Guid FileId {get;set;} = Guid.Empty;
    private File(){

    }

    public File(Post p){
        LinkedPost = p;
    }
}