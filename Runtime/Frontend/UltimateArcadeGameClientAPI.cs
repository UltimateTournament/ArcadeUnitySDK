using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace UltimateArcade
{
    public class UltimateArcadeGameClientAPI
    {
        private readonly string gameToken;
        private readonly string baseServerName;

        // baseServerName would be "staging.ultimatearcade.io" or "ultimatearcade.io"
        // userToken is game specific and depending on hosting method can be provided via different channels
        public UltimateArcadeGameClientAPI(string gameToken, string baseServerName)
        {
            this.gameToken = gameToken;
            this.baseServerName = baseServerName;
        }

        public GameInfo GetGameInfo()
        {
            string tokenJson = System.Text.Encoding.Default.GetString(Convert.FromBase64String(this.gameToken.Split('.')[1]));
            var address = JsonConvert.DeserializeObject<serverInfo>(tokenJson).Address;
            return new GameInfo { ServerAddress = address };
        }

        public IEnumerator GetUserInfo(Action<UserInfo> callback, Action<string> errorCallback)
        {
            using (var webReq = new UnityWebRequest("https://userapi." + this.baseServerName + "/games/player-profile"))
            {
                webReq.SetRequestHeader("Authorization", "Bearer " + this.gameToken);
                yield return webReq.SendWebRequest();

                switch (webReq.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        errorCallback("Error: " + webReq.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        errorCallback("HTTP Error: " + webReq.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        callback(JsonConvert.DeserializeObject<UserInfo>(webReq.downloadHandler.text));
                        break;
                }
            }
        }

    }

    class serverInfo
    {
        [JsonProperty(PropertyName = "addr")]
        public string Address { get; set; }
    }

    // will be thrown when trying to call any client methods while the token is expired.
    // when this happens the user should be redirected to the arcade launcher
    public class TokenExpiredException : Exception {}

    public class UserInfo
    {
        public string Name { get; set; }
        public string Tokens { get; set; }
        //TODO game-sepcific data like skin etc
    }

    public class GameInfo
    {
        public string ServerAddress { get; set; }
    }
}