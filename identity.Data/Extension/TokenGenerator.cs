using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using identity.Data.ErrorHandler;
using IdentityModel.Client;

namespace identity.Data.Extension
{
 public  class TokenGenerator
    {

        public static async Task<Tuple<TokenResponse,TokenResponse>> GetTokenResponse( string email,string password )
        {
           
            TokenResponse usertoken = null;
            TokenResponse tokenResponse = null;
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
                if (disco.IsError)
                {
                    throw new RestException(disco.Error);
                }

                tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "customer_complaint_client",
                    ClientSecret = "secret",

                    Scope = "customer_complaint",

                });
                if (tokenResponse.IsError)
                {
                    throw new RestException(tokenResponse.Error);
                }

                var user = new PasswordTokenRequest();
                user.Password = password;
                user.UserName = email;
                user.ClientId = "customer_complaint_client";
                user.SetBearerToken(tokenResponse.AccessToken);
                user.ClientSecret = "secret";
                user.Address = disco.TokenEndpoint;
                user.Scope = "customer_complaint";
                user.GrantType = "password";
                usertoken = await
                    client.RequestPasswordTokenAsync(user);
                if (usertoken.IsError)
                {
                    throw new RestException(usertoken.Error);
                }

               
            }
            return new Tuple<TokenResponse, TokenResponse>(tokenResponse,usertoken);
        }
    }
}
