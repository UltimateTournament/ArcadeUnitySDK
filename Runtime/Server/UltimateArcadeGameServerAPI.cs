using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateArcade.Server
{
    public class UltimateArcadeGameServerAPI
    {

        public UltimateArcadeGameServerAPI()
        {
        }

        public Task<ServerData> Init()
        {
            // get server token from environment or pod annotation etc and store it in instance
        }

        // you need to wait for this call to finish before exiting
        public Task Shutdown()
        {
        }

        public Task<PlayerInfo> ActivateUser(string playerToken)
        {
            // player token is signed JWT containing
            // * slipID
            // * userID
            // * server info (game, serverid)
        }

        public Task<PlayerInfo> SettleUserWithScore(string playerToken, int score)
        {
        }

        // report game-spefic metrics/events, like "player.joined", "player.found-secret", ...
        public void ReportKPI(string eventID, Dictionary<string,object> data)
        {
        }
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
        public string RandomSeed { get; set; }
    }

    public class PlayerInfo
    {
        public string Name { get; set; }
        public string PlayerID { get; set; }
        public string SlipID { get; set; }
        public int TokensInGame { get; set; }
    }

}