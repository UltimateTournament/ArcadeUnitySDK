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

    /// <summary>
    // use this seed to derive all randomness in your game to guarantee
    // that all players have the same fair experience
    /// </summary>
    public static string RandomSeed { get; set; }

    public static string PlayerToken { get; set; }

    public delegate void ClientReady(string token);
    private static ClientReady _clientReady;
    public static event ClientReady OnClientReady
    {
        add
        {
            _clientReady += value;
            if (!string.IsNullOrEmpty(PlayerToken))
            {
                value(PlayerToken);
            }
        }
        remove 
        {
            _clientReady -= value;
        }
    }

    public delegate void ServerReady(string seed);
    private static ServerReady _serverReady;
    public static event ServerReady OnServerReady
    {
        add
        {
            _serverReady += value;
            if (!string.IsNullOrEmpty(RandomSeed))
            {
                value(RandomSeed);
            }
        }
        remove
        {
            _serverReady -= value;
        }
    }


#if UNITY_WEBGL

    void Start()
    {
        StartCoroutine(this.initClient(0));

        var nm = this.GetComponent<NetworkManager>();
        var swt = this.GetComponent<SimpleWebTransport>();
        swt.sslEnabled = ExternalScriptBehavior.IsSecure();
        swt.port = (ushort)ExternalScriptBehavior.Port();
        nm.networkAddress = ExternalScriptBehavior.Hostname();
        UADebug.Log("Will connect to " + nm.networkAddress + ":" + swt.port);
        nm.StartClient();
    }

    private IEnumerator initClient(float waitTime)
    {
        UADebug.Log("Waiting for token");
        yield return new WaitForSeconds(waitTime);
        PlayerToken = ExternalScriptBehavior.Token();
        if (string.IsNullOrEmpty(PlayerToken))
        {
            StartCoroutine(this.initClient(0.1f));
        }
        else
        {
            UADebug.Log("got token "+ PlayerToken);
            _clientReady?.Invoke(PlayerToken);
        }
    }

#else

    private UltimateArcadeGameServerAPI api; 

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
        UADebug.Log("random seed: " + obj.RandomSeed);
        RandomSeed = obj.RandomSeed;
        _serverReady?.Invoke(RandomSeed);
    }

    private void onServerNotReady(string obj)
    {
        StartCoroutine(this.init(1));
    }
#endif

}
