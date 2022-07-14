using Newtonsoft.Json;

namespace Cqc.Helpers.GraphClient.User
{
    public class PasswordProfile
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("forceChangePasswordNextSignIn")]
        public bool ForceChangePasswordNextSignIn { get; set; }
    }
}
