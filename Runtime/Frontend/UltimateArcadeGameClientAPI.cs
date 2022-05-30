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
}