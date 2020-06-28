using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using identity.Data.Abstraction;
using identity.Data.Extension;
using identity.Data.Model;
using IdentityModel.Client;
using IdentityServer.DTO;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IHttpClientFactory clientFactory, IUser userRepository, IMapper mapper,
            IHttpContextAccessor contextAccessor, ILogger<AuthController> logger)
        {
            _clientFactory = clientFactory;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Signin([FromBody] LoginDto model)
        {
            UserDto userDto = null;
            TokenResponse usertoken = null;
            TokenResponse tokenResponse = null;
            Tuple<TokenResponse, TokenResponse> accessToken = null;

            try
            {
                var users = await _userRepository.GetUserByEmail(email: model.Email);
                if (users == null)
                {
                    return NotFound(new
                    {
                        message = "user not found",
                        code = 404,
                        success = false,
                        body = new {}
                    });
                }

                var loginUser = await _userRepository.Login(users, model.Password);
                if (loginUser == null)
                    return BadRequest(new { 
                        Message ="invalid email or password",
                        code = 400,
                        success = false,
                      body =  new {}
                      });
                accessToken = await TokenGenerator.GetTokenResponse(model.Email, model.Password);

                userDto = _mapper.Map<User, UserDto>(loginUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogDebug(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = " an error occured during login ",
                        code = 500,
                        success = false,
                        body = new { }
                    });
            }

            var returnUser = new ProfileDTO(userDto, accessToken.Item1, accessToken.Item2);
            return Ok(new
            {
                message = "login successful ",
                code = 200,
                status = true,
                body=returnUser
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            Tuple<TokenResponse, TokenResponse> accessToken = null;
            UserDto userDto = null;
            try
            {
                var checkEmail = await _userRepository.VerifyEmailExits(model.Email);
                if (checkEmail) 
                    
                   return BadRequest(new
                {
                    message = "user already exist",
                    code = 400,
                    success = false,
                        body = new { }
                });
                var user = _mapper.Map<RegisterDto, User>(model);
               await _userRepository.Register(user, model.Password);
               
              
                accessToken = await TokenGenerator.GetTokenResponse(model.Email, model.Password);
                userDto = _mapper.Map<RegisterDto, UserDto>(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogDebug(e.InnerException?.ToString() ?? e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        message =" an error occured while creating user",
                        code = 500,
                        success = false,
                        body =  new {}
                    });
            }

            var returnUser = new ProfileDTO(userDto, accessToken.Item1, accessToken.Item2);
            return Ok(new
            {
                message = "user successfully created",
                code = 200,
                success = true,
                body = returnUser,
                
            });
        }
    }
}