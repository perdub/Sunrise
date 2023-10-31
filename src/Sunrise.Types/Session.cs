using System.ComponentModel.DataAnnotations;

namespace Sunrise.Types;

public class Session{
    [Key]
    public string SessionId{get;init;}

    public Account Account{get;init;}

    //ip адреса, с которых на эту сессию были выполнены подключения
    public List<System.Net.IPAddress> IPAddresses{get;set;} = new ();
    
}