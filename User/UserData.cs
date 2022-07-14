using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cqc.Helpers.GraphClient.User
{
    public class UserData
    {
        [JsonProperty("accountEnabled")]
        public bool AccountEnabled { get; set; }
        
        [JsonProperty("creationType")]
        public string CreationType { get; set; }
        
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("passwordProfile")]
        public PasswordProfile PasswordProfile { get; set; }
        
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("identities")]
        public List<Identities> Identities { get; set; }

        [JsonProperty("passwordPolicies")]
        public string PasswordPolicies { get; set; }

    }
}
