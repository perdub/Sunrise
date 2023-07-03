using System.ComponentModel.DataAnnotations.Schema;

namespace Sunrise.Types;

public class Tag{
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int TagId {get;private set;}
    public string FullText {get;private set;}
    public string SearchText{get; private set;}

    public string Description{get;private set;}

    public int PostCount {get;private set;}
    
}