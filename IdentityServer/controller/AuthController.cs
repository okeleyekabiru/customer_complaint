using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using identity.Data.Abstraction;
using identity.Data.Model;
using IdentityModel.Client;
using IdentityServer.DTO;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Response.Model;

namespace IdentityServer.controller
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IUser _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public AuthController(IHttpClientFactory clientFactory,IUser userRepository,IMapper mapper,IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Signin([FromBody]LoginDto model)
        {
            UserDto userDto = null;
            TokenResponse tokenResponse = null;
            TokenResponse usertoken = null;

            try
            {

                
                var client = _clientFactory.CreateClient();
                
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
                if (disco.IsError)
                {
                    
                    return BadRequest(Responses<string>.BadResponse("an error occured while requesting for user token",
                        disco.Error));
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
                    return BadRequest(
                        Responses<string>.BadResponse("an error occured while requesting for client token",
                            tokenResponse.Error));
                }

                var user = new PasswordTokenRequest();
                user.Password = model.Password;
                user.UserName = model.Email;
                user.ClientId = "customer_complaint_client";
                user.SetBearerToken(tokenResponse.AccessToken);
                user.ClientSecret = "secret";
                user.Address = disco.TokenEndpoint;
                user.Scope = "customer_complaint";
                user.GrantType = "ResourceOwnerPassword";
                 usertoken = await
                    client.RequestPasswordTokenAsync(user);
                if (usertoken.IsError)
                {
                    var e = usertoken.Error;
                   
                    return BadRequest(Responses<string>.BadResponse("an error occured while requesting for user token",
                        usertoken.Error));
                }

                


                var users = await _userRepository.GetUserByEmail(email: model.Email);
                if (users == null)
                {
                    return NotFound(Responses<object>.NotFoundResponse("user not found",
                        new {error = "user does not exist"}));
                }

                var loginUser = await _userRepository.Login(users, model.Password);
                if (loginUser == null) return BadRequest(Responses<object>.BadResponse("invalid email or password",new {error = "login failed"}));

                userDto = _mapper.Map<User, UserDto>(loginUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Responses<object>.InternalServerResponsee(e.Message, new {error = e.Message}));
            }
            var  returnUser = new ProfileDTO(userDto,tokenResponse,usertoken);
            return Ok(Responses<ProfileDTO>.OkResponse("login successful", returnUser));
        }
    }

    

    
}