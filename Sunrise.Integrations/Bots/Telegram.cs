namespace Sunrise.Integrations;
using Telegram.Bot;
using Telegram.Bot.Types;

public class TelegramBot : IBot
{
    TelegramBotClient client;
    public Action<string, object> OnInputEvent { get; set; }
    async Task ProcessUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        OnInputEvent?.Invoke(update.Message.Text, update.Message.Chat.Id);
    }
    async Task ProcessError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Sunrise.Logger.Logger.Singelton.Write($"Telegram bot error, {exception.ToString()}");
    }

    //должен получить токен в виде строки
    public async Task Initialization(Func<object> token)
    {
        var q = (string)token();
        client = new TelegramBotClient(q);
        client.StartReceiving(ProcessUpdate, ProcessError);
    }

    public async Task Send(Func<object> id, string message)
    {
        long chatid = (long)id();
        await client.SendTextMessageAsync(chatid, message);
    }
}
