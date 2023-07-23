//todo: переделать это, добавив абстракций и так далее 

namespace Sunrise.Integrations.Bots;

using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Sunrise.Utilities;

public class TelegramBot
{
    TelegramBotClient _bot;
    //буфер для всех обновлений
    Dictionary<long, Queue<Update>> _temp = new Dictionary<long, Queue<Update>>();
    //получает обновление для чата через его айди
    async Task<Update> ReciveUpdate(long chatId)
    {
        while(true){
            bool contain = _temp.TryGetValue(chatId, out Queue<Update> upd);
            if(contain && upd.Count > 0){
                return upd.Dequeue();
            }
            await Task.Delay(100);
        }
    }
    async Task<string> getText(long id){
        //обертка для получения текста
        return (await ReciveUpdate(id)).Message.Text;
    }
    void sendText(string text, long id){
        _bot.SendTextMessageAsync(id, text).Wait();
    }

    public TelegramBot(string token)
    {
        _bot = new TelegramBotClient(token);
        _bot.StartReceiving(Update, Error);
    }
    //обрабатывает сообщения, вся логика бота в этом методе
    async Task Process(long chatid){
        while(true){
            string command = await getText(chatid);
            switch(command){
                #region /PING
                case "/ping":
                    await _bot.SendTextMessageAsync(chatid, "pong");
                    break;
                #endregion
                #region /WHOAMI
                case "/whoami":
                    SunriseContext c = new SunriseContext();
                    var me = c.Users.Where(q => q.TelegramAccountId == chatid).FirstOrDefault();
                    if(me==null){
                        sendText("Non-authorized", chatid);
                    }
                    else{
                        switch(me.Level){
                            case Types.PrivilegeLevel.Admin:
                                sendText(System.Text.Json.JsonSerializer.Serialize(me), chatid);
                                break;
                            case Types.PrivilegeLevel.Moderator:
                                sendText(System.Text.Json.JsonSerializer.Serialize(me.GetApiView()), chatid);
                                break;
                            case Types.PrivilegeLevel.Default:
                                sendText($"{me.Name}", chatid);
                                break;
                        }
                    }
                    break;
                #endregion
                case "/upload":
                    string tags = "";
                    await _bot.SendTextMessageAsync(chatid, "Please, enter a default tags(They will be applied to each post)");
                    tags = await getText(chatid);
                    while(true){
                        sendText("Please, send image, and text after.", chatid);

                        //получение картинки
                        var img = await ReciveUpdate(chatid);
                        if(img.Message.Text != null && img.Message.Text == "/stop"){
                            break;
                        }
                        var fileId = img.Message.Photo.Last().FileId;
                        Stream rawImg = new MemoryStream();
                        await _bot.DownloadFileAsync((await _bot.GetFileAsync(fileId)).FilePath, rawImg);
                        
                        sendText("Send tags to this post.", chatid);
                        var tagsInput = await getText(chatid);
                        if(tagsInput=="/stop"){
                            break;
                        }
                        SunriseContext sc = new ();
                        await sc.Upload(
                            sc.Users.Where(q => q.TelegramAccountId == chatid).FirstOrDefault().Id,
                            rawImg.ToByteArray(),
                            ".jpg",
                            tagsInput + ' ' + tags
                        );
                    }
                    break;
            }
        }
    }


    Types.User? getActiveUser(SunriseContext c, long chatId){
        return c.Users.Where(q => q.TelegramAccountId == chatId).FirstOrDefault();
    }

    async Task Update(ITelegramBotClient client, Update u, CancellationToken token){
        long id = u.Message?.Chat?.Id ?? -1;
        
        if(!_temp.Keys.Contains(id)){
            //добавляет новый чат для обновлений если ключ не найден
            var nq = new Queue<Update>();
            nq.Enqueue(u);
            _temp.Add(id, nq);
            Process(id);
        }
        else{
            //получает очередь и добавляет в неё
            _temp.TryGetValue(id, out Queue<Update> upd);
            upd.Enqueue(u);
        }

        /*SunriseContext context = new SunriseContext();
        if(u.Message?.Text?.Split()[0] == "/start"){
            //начало верификации

            //проверка на то что телеграм пользователя уже связанн с аккаунтом
            long account = u.Message.Chat.Id;
            var exsistingUser = context.Users.Where(x => x.TelegramAccountId == account).FirstOrDefault();
            if(exsistingUser!=null){
                await client.SendTextMessageAsync(account, $"You are already logged as {exsistingUser.Name}");
                return;
            }

            string key = u.Message.Text.Split()[^1];
            var a = context.Verify.Where(s => s.Key == key).FirstOrDefault();
            var user = context.Users.Find(a.UserId);
            user.TelegramAccountId = account;
            user.CheckedUser = true;
            await client.SendTextMessageAsync(user.TelegramAccountId, $"Great, you were verified on Sunrise.\n\nYour nickname - {user.Name}");
            context.Verify.Remove(a);
            context.SaveChanges();
            return;
        }

        if(u.Message?.Photo != null){

        }//*/
    }

    async Task Error(ITelegramBotClient client, Exception u, CancellationToken token){
        
    }
}