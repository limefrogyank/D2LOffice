using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace D2LOffice.Models
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        [JsonPropertyName("access_token")]
        required public string AccessToken { get; set; }

        [JsonProperty("expires_at")]
        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }
    }
}
