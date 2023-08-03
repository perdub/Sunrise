//todo: переделать это, добавив абстракций и так далее 

namespace Sunrise.Integrations.Telegram;

using Microsoft.EntityFrameworkCore;
using global::Telegram.Bot;
using global::Telegram.Bot.Types;
using Sunrise.Utilities;

public class TelegramBot
{
    ILogger<TelegramBot> _logger;
    TelegramBotClient _bot;
    //буфер для всех обновлений
    Dictionary<long, Queue<Update>> _temp = new Dictionary<long, Queue<Update>>();
    //получает обновление для чата через его айди

    //используется ли local bot server для тг бота
    // https://github.com/tdlib/telegram-bot-api
    bool isLocalServer = false;

    async Task<Update> ReciveUpdate(long chatId)
    {
        while (true)
        {
            bool contain = _temp.TryGetValue(chatId, out Queue<Update> upd);
            if (contain && upd.Count > 0)
            {
                return upd.Dequeue();
            }
            await Task.Delay(100);
        }
    }
    async Task<string> getText(long id)
    {
        //обертка для получения текста
        return (await ReciveUpdate(id)).Message.Text;
    }
    void sendText(string text, long id)
    {
        _bot.SendTextMessageAsync(id, text).Wait();
    }

    public TelegramBot(string token, ILogger<TelegramBot> logger, bool useLocalBotServer = false, string? localBotUrl = default)
    {
        _logger=logger;
        TelegramBotClientOptions options;
        if(useLocalBotServer){
            options = new TelegramBotClientOptions(token, localBotUrl);
            isLocalServer = true;
        }
        else{
            options = new TelegramBotClientOptions(token);
        }
        _bot = new TelegramBotClient(options);
        _bot.StartReceiving(Update, Error);
    }
    //обрабатывает сообщения, вся логика бота в этом методе
    async Task Process(long chatid)
    {
        while (true)
        {
            string command = await getText(chatid);
            switch (command)
            {
                #region /PING
                case "/ping":
                    await _bot.SendTextMessageAsync(chatid, "pong");
                    break;
                #endregion
                #region /WHOAMI
                case "/whoami":
                    SunriseContext c = new SunriseContext();
                    var me = c.Users.Where(q => q.TelegramAccountId == chatid).FirstOrDefault();
                    if (me == null)
                    {
                        sendText("Non-authorized", chatid);
                    }
                    else
                    {
                        switch (me.Level)
                        {
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
                    while (true)
                    {
                        sendText("Please, send image, and text after.", chatid);

                        //получение картинки
                        var img = await ReciveUpdate(chatid);
                        if (img.Message.Text != null && img.Message.Text == "/stop")
                        {
                            break;
                        }
                        Stream rawImg = new MemoryStream();
                        string targetExtension = ".jpg";
                        if (img.Message.Photo != null)
                        {
                            var fileId = img.Message.Photo.Last().FileId;
                            await DownloadOrReadFile((await _bot.GetFileAsync(fileId)).FilePath, rawImg);
                        }
                        else if(img.Message.Document != null){
                            var fi = img.Message.Document.FileId;
                            var fp = (await _bot.GetFileAsync(fi)).FilePath;
                            targetExtension = Path.GetExtension(fp);
                            await DownloadOrReadFile(fp, rawImg);
                        }
                        else if(img.Message.Video != null){
                            targetExtension = ".mp4";
                            await DownloadOrReadFile((await _bot.GetFileAsync(img.Message.Video.FileId)).FilePath, rawImg);
                        }


                        sendText("Send tags to this post.", chatid);
                        var tagsInput = await getText(chatid);
                        if (tagsInput == "/stop")
                        {
                            break;
                        }
                        SunriseContext sc = new();
                        await sc.Upload(
                            sc.Users.Where(q => q.TelegramAccountId == chatid).FirstOrDefault().Id,
                            rawImg.ToByteArray(),
                            targetExtension,
                            tagsInput + ' ' + tags
                        );
                        sendText("Succesful!", chatid);
                    }
                    sendText("Stop upload", chatid);
                    break;
                case "/mass_upload":
                    sendText("Enter tags. His will add to all new posts.", chatid);
                    string defaultTags = await getText(chatid);
                    sendText("Now you can start upload, or enter /stop to stop uploading", chatid);
                    while(true)
                    {
                        var message = await ReciveUpdate(chatid);
                        if(message.Message?.Text!=null && message.Message?.Text=="/stop")
                        {
                            sendText("Upload stop.", chatid);
                            break;
                        }

                        Stream rawData = new MemoryStream();
                        string targetExtension = ".jpg";

                        if(message.Message.Photo != null){
                            var fileId = message.Message.Photo.Last().FileId;
                            await DownloadOrReadFile((await _bot.GetFileAsync(fileId)).FilePath, rawData);
                        }
                        else if(message.Message.Document != null){
                            var fi = message.Message.Document.FileId;
                            var fp = (await _bot.GetFileAsync(fi)).FilePath;
                            targetExtension = Path.GetExtension(fp);
                            await DownloadOrReadFile(fp, rawData);
                            
                        }
                        else if(message.Message.Video != null){
                            targetExtension = ".mp4";
                            await DownloadOrReadFile((await _bot.GetFileAsync(message.Message.Video.FileId)).FilePath, rawData);
                        }
                        
                        SunriseContext sc = new();
                        await sc.Upload(
                            sc.Users.Where(q => q.TelegramAccountId == chatid).FirstOrDefault().Id,
                            rawData.ToByteArray(),
                            targetExtension,
                            defaultTags
                        );
                        sendText("Upload!", chatid);
                    }
                    break;
            }
        }
    }

    async Task DownloadOrReadFile(string path, Stream s){
        if(!s.CanWrite){
            throw new Exception("Fall to write to stream");
        }
        s.Position = 0;
        if(isLocalServer){
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            s.Write(bytes,0,bytes.Length);
        }
        else{
            await _bot.DownloadFileAsync(path, s);
        }
        s.Position = 0;
    }


    Types.User? getActiveUser(SunriseContext c, long chatId)
    {
        return c.Users.Where(q => q.TelegramAccountId == chatId).FirstOrDefault();
    }

    async Task Update(ITelegramBotClient client, Update u, CancellationToken token)
    {
        long id = u.Message?.Chat?.Id ?? -1;

        if (!_temp.Keys.Contains(id))
        {
            //добавляет новый чат для обновлений если ключ не найден
            var nq = new Queue<Update>();
            nq.Enqueue(u);
            _temp.Add(id, nq);
            Process(id);
        }
        else
        {
            //получает очередь и добавляет в неё
            _temp.TryGetValue(id, out Queue<Update> upd);
            upd.Enqueue(u);
        }
    }

    async Task Error(ITelegramBotClient client, Exception u, CancellationToken token)
    {
        _logger.Log(LogLevel.Error, u, u.Message);
    }
}