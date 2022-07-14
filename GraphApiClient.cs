using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cqc.Helpers.GraphClient.Configuration;
using Cqc.Helpers.GraphClient.User;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable InconsistentNaming - b2c
namespace Cqc.Helpers.GraphClient
{
    public class GraphApiClient : IGraphClient
    {
        private readonly GraphApiConfiguration _configuration;
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly string[] _scopes;

        public GraphApiClient(IOptions<GraphApiConfiguration> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException($"Graph Client requires {nameof(configuration)}'");
            }

            _configuration = configuration.Value;
            _scopes = new[] { $"{_configuration.ApiUrl}.default" };

            _confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(_configuration?.B2CClientId)
                .WithClientSecret(_configuration?.B2CClientSecret)
                .WithAuthority(new Uri(configuration?.Value.B2CAuthority))
                .Build();
        }

        /// <summary>
        /// Create user with specified user data
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="displayName"></param>
        /// <param name="firstName"></param>
        /// <param name="surname"></param>
        /// <returns>Id of the user created</returns>
        public async Task<string> CreateUser(string emailAddress, string displayName, string firstName, string surname)
        {
            var pf = new PasswordProfile
            {
                Password = PasswordGenerator.RandomPasswordGenerator(),
                ForceChangePasswordNextSignIn = false
            };
            var user = new UserData()
            {
                AccountEnabled = true,
                Identities = new List<Identities>()
                {
                    new Identities()
                    {
                        Issuer = _configuration.B2CTenant,
                        SignInType = "emailAddress",
                        IssuerAssignedId = emailAddress
                    }
                },
                
                CreationType = "LocalAccount",
                DisplayName = displayName,
                GivenName = firstName,
                Surname = surname,
                PasswordProfile = pf,
                PasswordPolicies = "DisablePasswordExpiration",
                
            };
            var json = JsonConvert.SerializeObject(user);

            AuthenticationResult token = await _confidentialClientApplication.AcquireTokenForClient(_scopes)
                .ExecuteAsync();

            if (token != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                var result = await apiCaller.PostToWebApiAndProcessResultASync($"{_configuration?.ApiUrl}v1.0/users", token.AccessToken, json);
                return result.SelectToken("id").ToString();
            }

            throw new Exception("Token failed to be acquired");
        }

        /// <summary>
        /// Check if a user exists by email address
        /// </summary>
        /// <param name="userEmailAddress">emailAddress to search</param>
        /// <returns>true if user exists</returns>
        public async Task<bool> UserExist(string userEmailAddress)
        {
            var token = await _confidentialClientApplication.AcquireTokenForClient(_scopes)
                .ExecuteAsync();

            if (token != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                JObject response = await apiCaller.GetRequestWebApiAndProcessResultASync($"{_configuration?.ApiUrl}v1.0/users/?$filter=identities/any(id:id/issuer eq '{_configuration.B2CTenant}' and id/issuerAssignedId eq '{userEmailAddress}')", token.AccessToken);
                return response.SelectToken("value").Any();
            }

            throw new Exception("Token failed to be acquired");
        }

    }
}