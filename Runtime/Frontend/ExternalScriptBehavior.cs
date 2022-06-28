
using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;


namespace Assets.Scripts.Core
{
    public class ExternalScriptBehavior
    {
#if UNITY_WEBGL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        [DllImport("__Internal")]
        public static extern void Log(string str);

        /// <summary>
        /// Tells the Ultimate Arcade parent window that the game session cannot continue, and the game client can be removed from the page.
        /// 
        /// If a player has already joined a game, this method will not release them, but it gives a better experience to players
        /// when the game encounters unresolvable errors like permanent network connection loss.
        /// </summary>
        /// <param name="reason"></param>
        [DllImport("__Internal")]
        public static extern void ReportErrorAndCloseGame(string reason);

        /// <summary>
        /// If the game is running on HTTP<b>S</b>
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern bool IsSecure();

        /// <summary>
        /// The host to connect to
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern string Hostname();

        /// <summary>
        /// The player's authentication token
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern string Token();

        /// <summary>
        /// Port to connect to
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern int Port();

        /// <summary>
        /// Used internally to find the UA servers for retrieving player information
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern string BaseApiServerName(); 

        /// <summary>
        /// Report game is done, so the game window can be closed
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern string CloseGame();

#else
        public static void CloseGame()
        {
            Debug.Log("close game");
        }
 
        public static void ReportErrorAndCloseGame(string reason)
        {
            Debug.Log("error closing game: " + reason);
        }
 
        public static void Log(string str)
        {
            Debug.Log(str);
        }
        public static bool IsSecure()
        {
            return false;
        }
        public static string Hostname()
        {
            return "localhost";
        }
        public static string Token()
        {
            return "";
        }
        public static string BaseApiServerName()
        {
            return "";
        }
        public static int Port()
        {
            return 7778;
        }
#endif

    }
}