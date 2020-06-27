using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.configuration
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("customer_complaint", "Complaint API")
            };
        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource("complaint_API", "Complaint API")
                {
                    Scopes ={ "customer_complaint"},
                     ApiSecrets = {new Secret("secret".ToSha256())},
                      Name = "customer_complaint",
                      ShowInDiscoveryDocument = true
                }
            };
        }
       
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "customer_complaint_client",
                    ClientName = "Customer Complaint",
                    ClientUri = "https://localhost:7001",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RequireConsent = false,
                    RedirectUris =
                    {
                        "https://localhost:7001",
                        "https://localhost:7001"
                    },

                    PostLogoutRedirectUris = { "https://localhost:7001" },
                    AllowedCorsOrigins = { "https://localhost:7001" },

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "customer_complaint", "complaint_API" ,StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,}
                }
                
};
    };
    }

