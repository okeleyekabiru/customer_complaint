using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity.Data
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = new[] {OidcConstants.GrantTypes.ClientCredentials},

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256(),DateTime.Now.AddDays(30))
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                }
            };
    }
}
