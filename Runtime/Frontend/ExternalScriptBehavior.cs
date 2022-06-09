
using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Assets.Scripts.Core
{
    public class ExternalScriptBehavior
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void Log(string str);

        [DllImport("__Internal")]
        public static extern bool IsSecure();

        [DllImport("__Internal")]
        public static extern string Hostname();

        [DllImport("__Internal")]
        public static extern string Token();

        [DllImport("__Internal")]
        public static extern int Port();

        [DllImport("__Internal")]
        public static extern string BaseApiServerName(); 

        [DllImport("__Internal")]
        public static extern string CloseGame();

#else
        public static void CloseGame()
        {
            Debug.Log("close game");
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