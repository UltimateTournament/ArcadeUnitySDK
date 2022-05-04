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

        public Task Init()
        {
            // get token from environment or pod annotation etc
        }

        public Task<PlayerInfo> ActivateUser(string playerToken)
        {
            // player token is signed JWT containing
            // * slipID
            // * userID
            // * server info (game, serverid)
        }
    }

    public class PlayerInfo
    {
        public string Name { get; set; }
        public string PlayerID { get; set; }
        public string SlipID { get; set; }
        public int TokensInGame { get; set; }
    }

}