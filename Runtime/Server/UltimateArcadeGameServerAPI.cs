using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using Arcade.UnitySDK.Server.Model;
using System.Text;
using Newtonsoft.Json;

namespace UltimateArcade.Server
{
    public class UltimateArcadeGameServerAPI
    {
        private string apiAddr;

        public UltimateArcadeGameServerAPI()
        {
            this.apiAddr = Environment.GetEnvironmentVariable("UAHV_ADDR") ?? "http://localhost:8083";
        }

        public IEnumerator<UnityWebRequestAsyncOperation> Init(Action<ServerData> callback, Action<string> errorCallback)
        {
            return httpCall("GET", "/api/server", null, null, webReq => JsonConvert.DeserializeObject<ServerData>(webReq.downloadHandler.text), errorCallback);
        }

        public IEnumerator<UnityWebRequestAsyncOperation> Shutdown(Action callback, Action<string> errorCallback)
        {
            return httpCall("POST", "/api/server/shutdown", null, null, _ => callback(), errorCallback);
        }

        public IEnumerator<UnityWebRequestAsyncOperation> ActivatePlayer(string playerToken, Action callback, Action<string> errorCallback)
        {
            return httpCall("POST", "/api/player/activate", playerToken, null, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Show that the session is still played, so it doesn't get automatically cancelled by the system.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> HeartbeatPlayer(string playerToken, Action callback, Action<string> errorCallback)
        {
            return httpCall("POST", "/api/player/heartbeat", playerToken, null, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Close the player session. They will receive whatever tokens they won within the game.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> SettlePlayer(string playerToken, Action callback, Action<string> errorCallback)
        {
            return httpCall("POST", "/api/player/settle", playerToken, null, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Close player session and report score. This should only be used for
        /// leaderboard games(where it's the only call that should be used to close the session)
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> ReportPlayerScore(string playerToken, long score, Action callback, Action<string> errorCallback)
        {
            var body = new PlayerReportScore(score);
            return httpCall("POST", "/api/player/report-score", playerToken, body, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Close player session as they lost against someone. The loser is identified by the auth header.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> DefeatPlayer(string playerToken, string winnerToken, Action callback, Action<string> errorCallback)
        {
            var body = new PlayerDefeat(winnerToken);
            return httpCall("POST", "/api/player/defeat", playerToken, body, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Close the player session because they lost. They lose some of their tokens.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> SelfDefeatPlayer(string playerToken, Action callback, Action<string> errorCallback)
        {
            return httpCall("POST", "/api/player/self-defeat", playerToken, null, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Show that the pool is still played, so it doesn't get automatically cancelled by the system.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> HeartbeatPool(string poolID, Action callback, Action<string> errorCallback)
        {
            var body = new PoolHeartbeat(poolID);
            return httpCall("POST", "/api/pool/heartbeat", null, body, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Lock the pool, so no other players can join it anymore.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> LockPool(string poolID, Action callback, Action<string> errorCallback)
        {
            var body = new PoolLock(poolID);
            return httpCall("POST", "/api/pool/lock", null, body, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Return the pool as if no winner can be determined.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> ReturnPool(string poolID, Action callback, Action<string> errorCallback)
        {
            var body = new PoolLock(poolID);
            return httpCall("POST", "/api/pool/return", null, body, _ => callback(), errorCallback);
        }

        /// <summary>
        /// Close the pool with a single winner. The winner passed in via the auth header player token.
        /// </summary>
        public IEnumerator<UnityWebRequestAsyncOperation> SettlePool(string poolID, Action callback, Action<string> errorCallback)
        {
            var body = new PoolLock(poolID);
            return httpCall("POST", "/api/pool/settle", null, body, _ => callback(), errorCallback);
        }

        private IEnumerator<UnityWebRequestAsyncOperation> httpCall(string method, string path, string authToken, object body, Action<UnityWebRequest> callback, Action<string> errorCallback)
        {
            using (var webReq = new UnityWebRequest(this.apiAddr + path, method))
            {
                if (authToken != null)
                {
                    webReq.SetRequestHeader("Authorization", "Bearer " + authToken);
                }
                if (body != null)
                {
                    var json = JsonConvert.SerializeObject(body);
                    webReq.SetRequestHeader("Content-Type", "application/json");
                    webReq.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                }
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
                        if (webReq.responseCode >= 400)
                        {
                            errorCallback("HTTP Status: " + webReq.responseCode);
                        }
                        else
                        {
                            callback(webReq);
                        }
                        break;
                }
            }
        }

    }

    class playerToken
    {
        [JsonProperty(PropertyName = "pid")]
        public string PlayerID { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public string SlipID { get; set; }
    }

    public class Debug
    {
        // NOTE: we'll log all SDK interactions, so you don't have to
        public static void Log(object message)
        {
        }
        public static void Log(object message, Object context)
        {
        }
    }

    public class ServerData
    {
        [JsonProperty(PropertyName = "random_seed")]
        public string RandomSeed { get; set; }
    }


}