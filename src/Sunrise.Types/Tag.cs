using System.ComponentModel.DataAnnotations;

namespace Sunrise.Types;

public class Tag{
    [Key]
    public int TagId{get;private set;}

    public string TagText{get;set;}

    public string TagDescription {get;set;}

    public List<Post> Posts{get;set;}

    private Tag(){

    }

    public Tag(string tag){
        TagText = TagDescription = tag;
    }
}