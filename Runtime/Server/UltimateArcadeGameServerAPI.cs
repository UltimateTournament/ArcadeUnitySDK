using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Agones;
using ArcadeSlipManager;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Newtonsoft.Json;

namespace UltimateArcade.Server
{
    public class UltimateArcadeGameServerAPI
    {
        private AgonesSDK agones;
        private string? serverToken;
        private string? externalAddress;
        private SlipManager.SlipManagerClient smClient;

        public UltimateArcadeGameServerAPI()
        {
            this.agones = new AgonesSDK();
            var slipManagerAddr = System.Environment.GetEnvironmentVariable("SLIP_MANAGER_ADDR");
            if (slipManagerAddr == null)
            {
                slipManagerAddr = "slip-manager.slip-manager.svc.cluster.local:9090";
            }
            // if we switch to .NET core we need this
            // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var defaultMethodConfig = new MethodConfig
            {
                RetryPolicy = new RetryPolicy
                {
                    MaxAttempts = 3,
                    InitialBackoff = TimeSpan.FromSeconds(0.1),
                    MaxBackoff = TimeSpan.FromSeconds(1),
                    BackoffMultiplier = 2,
                    RetryableStatusCodes = { StatusCode.Unavailable }
                }
            };
            ServiceConfig svcConfig = new ServiceConfig();
            svcConfig.MethodConfigs.Add(defaultMethodConfig);
            var channel = GrpcChannel.ForAddress("http://" + slipManagerAddr, new GrpcChannelOptions { ServiceConfig = svcConfig });
            this.smClient = new SlipManager.SlipManagerClient(channel);
        }

        private static readonly string[] neededAnnotations = new[] { "server-token", "random-seed", "external-address" };

        public Task<ServerData> Init()
        {
            var wait = new AutoResetEvent(false);
            var randomSeed = "";
            this.agones.WatchGameServer(gs =>
            {
                if (gs.ObjectMeta.Annotations.Keys.Intersect(neededAnnotations).Count() == neededAnnotations.Length)
                {
                    this.serverToken = gs.ObjectMeta.Annotations["server-token"];
                    this.externalAddress = gs.ObjectMeta.Annotations["external-address"];
                    randomSeed = gs.ObjectMeta.Annotations["random-seed"];
                    wait.Set();
                }
            });
            return Task.Run(() =>
            {
                wait.WaitOne();
                return new ServerData
                {
                    RandomSeed = randomSeed,
                    ExternalAddress = this.externalAddress,
                };
            });
        }

        // you need to wait for this call to finish before exiting
        public async Task<bool> Shutdown()
        {
            var res = await this.agones.ShutDownAsync();
            return res.StatusCode == Grpc.Core.StatusCode.OK;
        }

        public async Task ActivateUser(string playerToken)
        {
            // request is empty because all necessary data is in the playerToken
            await this.smClient.ActivateSlipAsync(new ActivateSlipReq(), headers: CreateAuthHeaders(playerToken));
            return;
        }

        private Metadata CreateAuthHeaders(string playerToken)
        {
            var headers = new Metadata();
            headers.Add("authorization", "bearer " + this.serverToken);
            headers.Add("x-player-token", "bearer " + playerToken);
            return headers;
        }

        // should be called for all players in leaderboard games
        public async Task FinishSessionWithScore(string playerToken, int score)
        {
            await this.smClient.ReportScoreForLeaderboardAsync(new ReportScoreForLeaderboardReq { Score = score }, headers: CreateAuthHeaders(playerToken));
        }

        // should be called when a user loses against another player in a non-leaderboard game
        public async Task FinishSessionWithLoss(string loserPlayerToken, string winnerPlayerToken)
        {
            string tokenJson = System.Text.Encoding.Default.GetString(Convert.FromBase64String(winnerPlayerToken.Split('.')[1]));
            var winnerToken = JsonConvert.DeserializeObject<playerToken>(tokenJson);
            await this.smClient.SlipDefeatedAsync(new SlipDefeatedReq { WinningPlayerID = winnerToken.PlayerID, WinningPlayerSlipID = winnerToken.SlipID }, headers: CreateAuthHeaders(loserPlayerToken));
        }

        // should be called when a user leaves a non-leaderboard game without losing
        public async Task FinishSession(string playerToken)
        {
            // request is empty because all necessary data is in the playerToken
            await this.smClient.SettleSlipAsync(new SettleSlipReq(), headers: CreateAuthHeaders(playerToken));
        }

        // report game-spefic metrics/events, like "player.joined", "player.found-secret", ...
        public void ReportKPI(string eventID, Dictionary<string, object> data)
        {
        }
    }

    class playerToken
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "pid")]
        public string PlayerID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "sid")]
        public string SlipID { get; set; }
    }

    public class Debug
    {
        // NOTE: we'll log all SDK interactions, so you don't have to
        public static void Log(object message)
        {
        }
        public static void Log(object message, Object context)
        {
        }
    }

    public class ServerData
    {
        public string RandomSeed { get; set; }
        public string ExternalAddress { get; set; }
    }


}