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
using System.IO;

namespace Arcade.UnitySDK.Server.Model
{
    /// <summary>
    /// PlayerDefeat
    /// </summary>
    public partial class PlayerDefeat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDefeat" /> class.
        /// </summary>
        /// <param name="winnerToken">winnerToken (required).</param>
        public PlayerDefeat(string winnerToken = default(string))
        {
            // to ensure "winnerToken" is required (not null)
            if (winnerToken == null)
            {
                throw new InvalidDataException("winnerToken is a required property for PlayerDefeat and cannot be null");
            }
            else
            {
                this.WinnerToken = winnerToken;
            }
        }

        /// <summary>
        /// Gets or Sets WinnerToken
        /// </summary>
        [JsonProperty(PropertyName = "winner_token")]
        public string WinnerToken { get; set; }

    }
}
