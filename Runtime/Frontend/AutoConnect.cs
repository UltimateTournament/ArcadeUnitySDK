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

    void Start()
    {
        var nm = this.GetComponent<NetworkManager>();
        nm.StartServer();
        this.api = new UltimateArcadeGameServerAPI();
        StartCoroutine(this.init(0));
    }

    private IEnumerator init(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return api.Init(this.ServerReady, this.ServerNotReady);
    }

    private void ServerReady(ServerData obj)
    {
        UADebug.Log("random seed: "+obj.RandomSeed);
        //TODO use this seed to derive all randomness in your game to guarantee
        // that all players have the same fair experience
    }

    private void ServerNotReady(string obj)
    {
        StartCoroutine(this.init(1));
    }
#endif

}
