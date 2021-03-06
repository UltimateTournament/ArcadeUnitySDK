/* 
 * Ultimate Arcade Game Server API
 *
 * This exposes all APIs that are available to game servers.  All APIs that interact with a player session (aka slip), identify the player by their token that  should be passed in the `Authorization: Bearer ${TOKEN}` header' and which you should receive in the player's first message from your frontend. The fronend will get it via the Ultimate Arcade SDK. 
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using Newtonsoft.Json;

namespace Arcade.UnitySDK.Server.Model
{
    public partial class PlayerInfo
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "player_id")]
        public string PlayerID { get; set; }
    }
}
