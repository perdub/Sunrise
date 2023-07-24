namespace Sunrise.Types;
using Sunrise.Utilities;
public class Verify{
    public Guid Id {get;private set;}
    public Guid UserId {get;private set;}

    public string Key {get;private set;}

    private Verify(){}

    public Verify(User user)
    {
        UserId = user.Id;
        Id = Guid.NewGuid();
        Key = SunToken.GetToken(16,17);
    }
}