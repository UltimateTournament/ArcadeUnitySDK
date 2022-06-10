using Assets.Scripts.Core;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;
using UltimateArcade.Frontend;
using UltimateArcade.Server;
using System;
using System.Threading;
using System.Collections;

public class AutoConnect : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR

    void Start()
    {
        var nm = this.GetComponent<NetworkManager>();
        var swt = this.GetComponent<SimpleWebTransport>();
        swt.sslEnabled = ExternalScriptBehavior.IsSecure();
        swt.port = (ushort)ExternalScriptBehavior.Port();
        nm.networkAddress = ExternalScriptBehavior.Hostname();
        nm.StartClient();
    }

#else

    private UltimateArcadeGameServerAPI api; 
    public delegate void ServerReady();
    public static event ServerReady OnServerReady;

    /// <summary>
    // use this seed to derive all randomness in your game to guarantee
    // that all players have the same fair experience
    /// </summary>
    public static string RandomSeed { get; set; }

    void Start()
    {
        var nm = this.GetComponent<NetworkManager>();
        var swt = this.GetComponent<SimpleWebTransport>();
        var portStr = Environment.GetEnvironmentVariable("PORT");
        if (portStr != null)
        {
            var port = int.Parse(portStr);
            swt.port = (ushort)port;
        }
        nm.StartServer();
        this.api = new UltimateArcadeGameServerAPI();
        StartCoroutine(this.init(0));
    }

    private IEnumerator init(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return api.Init(this.onServerReady, this.onServerNotReady);
    }

    private void onServerReady(ServerData obj)
    {
        UADebug.Log("random seed: "+obj.RandomSeed);
        OnServerReady();
    }

    private void onServerNotReady(string obj)
    {
        StartCoroutine(this.init(1));
    }
#endif

}
