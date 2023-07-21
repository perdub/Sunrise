namespace Sunrise.Types;
using Sunrise.Utilities;
public class Verify{
    public Guid Id {get;private set;}
    public User User {get;private set;}

    public string Key {get;private set;}

    private Verify(){}

    public Verify(User user)
    {
        User = user;
        Id = Guid.NewGuid();
        Key = SunToken.GetToken(16,17);
    }
}