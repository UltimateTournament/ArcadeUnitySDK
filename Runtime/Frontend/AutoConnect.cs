using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;
using System;
using System.Threading;
using System.Collections;

namespace UltimateArcade
{
    public class AutoConnect : NetworkBehaviour
    {

        /// <summary>
        // use this seed to derive all randomness in your game to guarantee
        // that all players have the same fair experience
        /// </summary>
        public static string RandomSeed { get; set; }

        public static string PlayerToken { get; set; }

        protected UltimateArcadeGameServerAPI serverApi { private set; get; }
        protected UltimateArcadeGameClientAPI clientApi { private set; get; }

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



        void Start()
        {
#if UNITY_WEBGL
            StartCoroutine(this.initClient(0));

            var nm = this.GetComponent<NetworkManager>();
            var swt = this.GetComponent<SimpleWebTransport>();
            swt.sslEnabled = ExternalScriptBehavior.IsSecure();
            swt.port = (ushort)ExternalScriptBehavior.Port();
            nm.networkAddress = ExternalScriptBehavior.Hostname();
            UADebug.Log("Will connect to " + nm.networkAddress + ":" + swt.port);
            nm.StartClient();
#else
            var nm = this.GetComponent<NetworkManager>();
            var swt = this.GetComponent<SimpleWebTransport>();
            var portStr = Environment.GetEnvironmentVariable("PORT");
            if (portStr != null)
            {
                var port = int.Parse(portStr);
                swt.port = (ushort)port;
            }
            nm.StartServer();
            this.serverApi = new UltimateArcadeGameServerAPI();
            StartCoroutine(this.initServer(0));
#endif
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
                UADebug.Log("got token " + PlayerToken);
                this.clientApi = new UltimateArcadeGameClientAPI(PlayerToken, ExternalScriptBehavior.BaseApiServerName());
                onClientReady();
            }
        }

        protected virtual void onClientReady()
        {
            _clientReady?.Invoke(PlayerToken);
        }

        private IEnumerator initServer(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            yield return serverApi.Init(this.onServerReady, this.onServerNotReady);
        }

        protected virtual void onServerReady(ServerData obj)
        {
            UADebug.Log("random seed: " + obj.RandomSeed);
            RandomSeed = obj.RandomSeed;
            _serverReady?.Invoke(RandomSeed);
        }

        private void onServerNotReady(string obj)
        {
            StartCoroutine(this.initServer(1));
        }

    }
}
