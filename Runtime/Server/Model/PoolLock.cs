/* 
 * Ultimate Arcade Game Server API
 *
 * This exposes all APIs that are available to game servers.  All APIs that interact with a player session (aka slip), identify the player by their token that  should be passed in the `Authorization: Bearer ${TOKEN}` header' and which you should receive in the player's first message from your frontend. The fronend will get it via the Ultimate Arcade SDK. 
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System.IO;
using Newtonsoft.Json;

namespace Arcade.UnitySDK.Server.Model
{
    /// <summary>
    /// PoolLock
    /// </summary>
    public partial class PoolLock 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PoolLock" /> class.
        /// </summary>
        /// <param name="poolId">poolId (required).</param>
        public PoolLock(string poolId = default(string))
        {
            // to ensure "poolId" is required (not null)
            if (poolId == null)
            {
                throw new InvalidDataException("poolId is a required property for PoolLock and cannot be null");
            }
            else
            {
                this.PoolId = poolId;
            }
        }

        /// <summary>
        /// Gets or Sets PoolId
        /// </summary>
        [JsonProperty(PropertyName = "pool_id")]
        public string PoolId { get; set; }

    }
}
