using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateArcade.Frontend
{
    public class UltimateArcadeGameClientAPI
    {
        // baseServerName would be "staging.ultimatearcade.io" or "ultimatearcade.io"
        // userToken is game specific and depending on hosting method can be provided via different channels
        public UltimateArcadeGameClientAPI(string userToken, string baseServerName)
        {
        }

        public Task<UserInfo> GetUserInfo()
        {
        }

        public Task<Match> MatchmakeLeaderboard(string leaderboardID)
        {
        }

        // returns some old (closed), some open and potentially some future leaderboards
        public Task<List<Leaderboard>> GetLeaderboards()
        {
        }
    }

    public class UserInfo
    {
        public string Name { get; set; }
        public string Tokens { get; set; }
        //TODO game-sepcific data like skin etc
    }

    public class Match
    {
        // opaque token you need to pass to the game server
        public string GameToken { get; set; }
        // websocket address to connect to e.g. wss://my-server.ua.io:7123/ws
        public string ServerAddress { get; set; }
    }

    public class Leaderboard
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int EntryPrice { get; set; }
        public int TokensToWin { get; set; }
        public DateTime OpenAt { get; set; }
        public DateTime CloseAt { get; set; }
        public List<Entry> Board { get; set; }
    }

    public class Entry
    {
        public string PlayerName { get; set; }
        // could be the same for multiple players
        public int Rank { get; set; }
        public int Score { get; set; }
        // what they got if the leaderboard is closed
        public int? TokensWon { get; set; }
    }
}