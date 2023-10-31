using System.Text;
using Sunrise.Integrations.Vk.Types;

namespace Sunrise.Integrations.Vk;

public class VkBot
{

    string accessToken = "";
    long groupId = 0;

    string longPollServerAddres = "";
    string longPollKey = "";
    string ts = "";



    HttpClient client;


    public VkBot(string accessToken, long groupId)
    {
        this.accessToken = accessToken;
        this.groupId = groupId;

        client = new HttpClient();
    }

    public async Task ReciveUpdates()
    {
        while (true)
        {
            var res = await client.GetAsync(buildUrlToGetUpdates());
            var strResult = await res.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateResult>(strResult);

            if (result == null)
            {
                throw new Exception("Result is null.");
            }

            ts = result.Ts;

            if (result.ErrorStatus == 2 || result.ErrorStatus == 3)
            {
                await UpdateLongPollServer();
            }
            if (result.ErrorStatus == 1)
            {
                ts = result.Ts;
            }
            if (result.ErrorStatus != 0)
            {
                continue;
            }

            if (result.Updates != null)
            {
                foreach (var upd in result.Updates)
                {
                    Message m = upd.Object.Message;
                    var clientId = m.FromId;

                    SunriseContext sc = new();

                    Sunrise.Types.User? u = sc.Users.Where(a => a.VkAccountId == clientId).FirstOrDefault();

                    int chatId = (int)m.PeerId - 2000000000;
                    //ВРОДЕ КАК ид чата можно найти так(по моим наблюдениям)
                    //как на самом деле я не имею ни малейшего понятия потому что документация апи вк ебучее говно которое делали дебилы(уж извините но это так)

                    var text = m.Text;

                    if (!string.IsNullOrEmpty(text))
                    {
                        switch (text)
                        {
                            case "/ping":
                                await sendTextMessage("pong!", chatId);
                                break;
                            case "/whoami":
                                if (u == null)
                                {
                                    sendTextMessage("Non-authorized", chatId);
                                }
                                else
                                {
                                    switch (u.Level)
                                    {
                                        case Sunrise.Types.PrivilegeLevel.Admin:
                                            sendTextMessage(System.Text.Json.JsonSerializer.Serialize(u), chatId);
                                            break;
                                        case Sunrise.Types.PrivilegeLevel.Moderator:
                                            sendTextMessage(System.Text.Json.JsonSerializer.Serialize(u.GetApiView()), chatId);
                                            break;
                                        case Sunrise.Types.PrivilegeLevel.Default:
                                            sendTextMessage($"{u.Name}", chatId);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        foreach (var i in m.Attachments)
                        {
                            if (i.Wall != null)
                            {
                                foreach (var j in i.Wall.Attachments)
                                {
                                    var p = j.Photo;
                                    await ProcessPhoto(p, sc, u, chatId);
                                }
                            }
                            if (i.Photo != null)
                            {
                                await ProcessPhoto(i.Photo, sc, u, chatId);
                            }
                        }
                    }
                }
            }
        }
    }

    async Task ProcessPhoto(Photo p, SunriseContext sc, Sunrise.Types.User u, int chatId)
    {
        var url = p.Sizes.OrderByDescending(a => a.Height).FirstOrDefault().Url;

        var img = await downloadImage(url);

        await sc.Upload(
            u.Id,
            img,
            Path.GetExtension(url.LocalPath),
            "tag_me"
        );

        sendTextMessage("Uploaded!", chatId);
    }

    public async Task UpdateLongPollServer()
    {
        var res = await client.GetAsync(buildUrlToGetLongPoll());
        var tempResult = await res.Content.ReadAsStringAsync();

        var obj = System.Text.Json.JsonSerializer.Deserialize<TopLevelLongPollResult>(tempResult);
        var result = obj.response;

        if (result == null)
        {
            throw new Exception("Fall to get Long Pool info.");
        }

        longPollServerAddres = result.server;
        longPollKey = result.key;
        ts = result.ts;

    }

    string buildUrlToGetLongPoll()
    {
        StringBuilder bld = new StringBuilder();
        bld.Append("https://api.vk.com/method/groups.getLongPollServer?v=5.131&group_id=");
        bld.Append(groupId);
        bld.Append("&access_token=");
        bld.Append(accessToken);
        return bld.ToString();
    }

    string buildUrlToGetUpdates()
    {
        StringBuilder bld = new StringBuilder();
        bld.Append(longPollServerAddres);
        bld.Append("?act=a_check&wait=25&key=");
        bld.Append(longPollKey);
        bld.Append("&ts=");
        bld.Append(ts);
        return bld.ToString();
    }

    string buildTextMessage(string text, int chatId)
    {
        StringBuilder bld = new StringBuilder();
        bld.Append("https://api.vk.com/method/messages.send?message=");
        bld.Append(text);
        bld.Append("&chat_id=");
        bld.Append(chatId);
        bld.Append("&random_id=");
        bld.Append(Random.Shared.Next());
        bld.Append("&v=5.131&access_token=");
        bld.Append(accessToken);
        return bld.ToString();
    }
    async Task sendTextMessage(string text, int chatId)
    {
        var u = buildTextMessage(text, chatId);
        var res = await client.GetAsync(u);
        res.EnsureSuccessStatusCode();
    }

    async Task<byte[]> downloadImage(Uri url)
    {
        var res = await client.GetByteArrayAsync(url);
        return res;
    }
}