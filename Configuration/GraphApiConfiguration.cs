using System;
using System.Globalization;

namespace Cqc.Helpers.GraphClient.Configuration
{
    public class GraphApiConfiguration
    {
        public string B2CAuthority => B2CInstance + B2CTenant;

        public string B2CInstance { get; set; }
        public string B2CTenant { get; set; }
        public string B2CClientId { get; set; }
        public string B2CClientSecret { get; set; }
        public string ApiUrl { get; set; }
    }
}

