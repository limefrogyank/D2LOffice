using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models
{
    public class GoogleAuthTokenInfo
    {
        [JsonProperty("access_token")]
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }

        [JsonProperty("scope")]
        [JsonPropertyName("scope")]
        public required string Scope { get; set; }

        [JsonProperty("token_type")]
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }

        [JsonProperty("id_token")]
        [JsonPropertyName("id_token")]
        public required string IdToken { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTimeOffset? Expires { get; set; }
        
    }
}
