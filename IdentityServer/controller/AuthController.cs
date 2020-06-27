using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityServer.controller
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Signin([FromBody]LoginViewModel model)
        {
            var client = _clientFactory.CreateClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7005");
            if (disco.IsError)
            {
                return BadRequest(disco.Error);
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "customer_complaint_client",
                ClientSecret = "secret",

                Scope = "customer_complaint",
                
            });
            var user = new PasswordTokenRequest();
            user.Password = model.Password;
            user.UserName = model.UserName;
            user.ClientId = "customer_complaint_client";
            user.Address = disco.TokenEndpoint;
           user.Scope = "customer_complaint";
           var usertoken = await
               client.RequestPasswordTokenAsync(user);
           if (usertoken.IsError)
           {
               return BadRequest(tokenResponse.Error);
            }
            if (tokenResponse.IsError)
            {
                return BadRequest(tokenResponse.Error);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // var apiClient = _clientFactory.CreateClient();
            // apiClient.SetBearerToken(tokenResponse.AccessToken);
            //
            // var response = await apiClient.GetAsync("https://localhost:6001/identity");
            // if (!response.IsSuccessStatusCode)
            // {
            //     Console.WriteLine(response.StatusCode);
            // }
            // else
            // {
            //     var content = await response.Content.ReadAsStringAsync();
            //     Console.WriteLine(JArray.Parse(content));
            // }

            //user login

            return BadRequest("Invalid username or password.");
        }
    }

    public class ProfileViewModel
    {
        public ProfileViewModel(object result, TokenResponse tokenResponse)
        {
            
        }
    }

    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}