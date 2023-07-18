namespace Sunrise.Integrations;

public enum State
{
    //состояние перед подтверждением сессии
    Verify=0,
    //загрузка через бота
    Upload=1,
    //проверка постов
    Review=2
}