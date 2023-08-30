using System.Text;
using Sunrise.Integrations.Vk.Types;

namespace Sunrise.Integrations.Vk;

public class VkBot{

    string accessToken = "";
    long groupId = 0;

    string longPollServerAddres = "";
    string longPollKey = "";
    string ts = "";

    

    HttpClient client;


    public VkBot(string accessToken, long groupId){
        this.accessToken = accessToken;
        this.groupId = groupId;

        client = new HttpClient();
    }

    public async Task ReciveUpdates(){
        while(true){
            var res = await client.GetAsync(buildUrlToGetUpdates());
            var result = await res.Content.ReadFromJsonAsync<UpdateResult>();

            if(result==null){
                throw new Exception("Result is null.");
            }

            if(result.ErrorStatus == 2 || result.ErrorStatus == 3){
                await UpdateLongPollServer();
            }
            if(result.ErrorStatus == 1){
                ts = result.Ts;
            }
            if(result.ErrorStatus!=0){
                continue;
            }

            if(result.Updates != null){
                foreach(var upd in result.Updates){
                    var clientId = upd.Object.Message.FromId;
                }
            }
        }
    }

    public async Task UpdateLongPollServer(){
        var res = await client.GetAsync(buildUrlToGetLongPoll());
        var result = await res.Content.ReadFromJsonAsync<LongPollResult>();

        if(result == null){
            throw new Exception("Fall to get Long Pool info.");
        }

        longPollServerAddres = result.Server.ToString();
        longPollKey = result.Key;
        ts = result.Ts;

    }

    string buildUrlToGetLongPoll(){
        StringBuilder bld = new StringBuilder();
        bld.Append("https://api.vk.com/method/groups.getLongPollServer?v=5.131&group_id=");
        bld.Append(groupId);
        bld.Append("&access_token=");
        bld.Append(accessToken);
        return bld.ToString();
    }

    string buildUrlToGetUpdates(){
        StringBuilder bld = new StringBuilder();
        bld.Append(longPollServerAddres);
        bld.Append("?act=a_check&time=25&key=");
        bld.Append(longPollKey);
        bld.Append("&ts=");
        bld.Append(ts);
        return bld.ToString();
    }
}