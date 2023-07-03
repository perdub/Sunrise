namespace Sunrise.Types;

//представляет User для публичного api
public class UserApiView : ApiView
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public DateTime AccountCreationTime { get; set; }
}