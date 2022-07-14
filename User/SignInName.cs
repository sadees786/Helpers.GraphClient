using Newtonsoft.Json;

namespace Cqc.Helpers.GraphClient.User
{
    public class Identities
    {
        [JsonProperty("signInType")]
        public string SignInType { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("issuerAssignedId")]
        public string IssuerAssignedId { get; set; }
    }
}
