using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateArcade.Frontend
{
    public class UltimateArcadeGameClientAPI
    {
        public UltimateArcadeGameClientAPI()
        {
            // TODO, take in 
            // * server base URL
            // * auth token (signed JWT)
            //   * game-id is derived from token 

        }

        public Task<Collection<Lobbies>> GetAvailableLobbies()
        {
        }
    }

    public class Lobby
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}