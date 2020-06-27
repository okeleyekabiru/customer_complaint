using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using identity.Data.Abstraction;
using IdentityModel.Client;
using IdentityServer.DTO;
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
        private readonly IUser _userRepository;

        public AuthController(IHttpClientFactory clientFactory,IUser userRepository)
        {
            _clientFactory = clientFactory;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Signin([FromBody]LoginDto model)
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
            user.UserName = model.Email;
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
                return BadRequest(ModelState.ValidationState);
            }

            var loginUser = await _userRepository.Login();

            return BadRequest("Invalid username or password.");
        }
    }

    

    
}