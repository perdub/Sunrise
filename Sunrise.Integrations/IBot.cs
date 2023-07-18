namespace Sunrise.Integrations;
//интерфейс управления ботом
public interface IBot
{
    //инициализация(создания клиента для телешрамма и тд)
    Task Initialization(Func<object> token);

    //эвент при вводе от пользователя
    //первый параметер - текст, второй - обьект индефикации, с ним можно обращатся к этому пользователю дальше
    Action<string, object> OnInputEvent {get;set;}
    
    Task Send(Func<object> id, string message);
}