//представляет собой сессию, которая содержит информацию о юзере и различную техническую информацию

namespace Sunrise.Types;

public class Session
{
    //уникальный ид сессии
    public string SessionId{get;private set;}
    //ид пользователя
    public User User{get; set;}
    //может ли сессия закончится(включён ли флажок запомнить меня или чтото в этом роде)
    public bool CanExpires{get;private set;} = false;
    //дата и время последнего взаимодействия с сессией
    public DateTime LastActivity {get; set;}
    //время до истечения сессии
    public TimeSpan SessionInactive {get;private set;}
    private Session(){}
    public Session(User user)
    {
        User = user;
        SessionId = SunToken.GetToken();
        LastActivity = DateTime.UtcNow;
    }
    public Session(User user, TimeSpan inactive) : this(user)
    {
        CanExpires = true;
        SessionInactive = inactive;
    }

    public bool IsActive()
    {
        return CanExpires ? (LastActivity+SessionInactive>DateTime.UtcNow) : true;
    }

    //обновляет дату и время последней активности
    public void UpdateActivity()
    {
        LastActivity = DateTime.UtcNow;
    }

    //обновляет время, а затем вызывает делегат. Делегат должен обновлять этот обьект в контексте базы данных
    public void UpdateActivity(Action a)
    {
        UpdateActivity();
        a.Invoke();
    }

}