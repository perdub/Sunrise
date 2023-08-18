using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sunrise.Types;

public class Tag{
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    //индефикатор тега(каждый тег представляется уникальным числом)
    public int TagId {get; set;}
    //текстовое представление, используещеесе на странице (1 girl)
    public string FullText {get; set;}
    //представление, использующеесе для поиска (1_girl)
    public string SearchText{get;  set;}

    //описание тега
    public string Description{get; set;}
    //количество постов с этим тегом
    public int PostCount {get; set;}

    //посты с этим тегом
    [JsonIgnore]
    public List<Post> Post {get;set;}

    public Tag(string tagSearch) : this()
    {
        FullText = Description = SearchText = tagSearch;
        PostCount = 0;
    }
    private Tag(){
        Post = new List<Post>();}
    
}