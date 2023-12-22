using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sunrise.Types;

public class Tag{
    [Key]
    public int TagId{get;private set;}

    public int PostCount{get;set;}

    public string TagText{get;set;}

    public string TagDescription {get;set;}
    [JsonIgnore]
    public List<Post> Posts{get;set;} = new List<Post>();

    private Tag(){

    }

    public Tag(string tag){
        TagText = TagDescription = tag;
    }
}