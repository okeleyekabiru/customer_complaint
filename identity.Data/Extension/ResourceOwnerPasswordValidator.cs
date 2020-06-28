using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using identity.Data.Abstraction;
using identity.Data.Model;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace identity.Data.Extension
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        //repository to get user from db
        private readonly IUser _userRepository;


        public ResourceOwnerPasswordValidator(IUser userRepository)
        {
            _userRepository = userRepository; //DI
        }

        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //get your user model from db (by username - in my case its email)
                var user = await _userRepository.GetUserByEmail(context.UserName);

                if (user != null)
                {
                    //check if password match - remember to hash password if stored as hash in db
                    var contextPassword =
                        new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, context.Password);
                    if (contextPassword == PasswordVerificationResult.Success)
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.Id,
                            authenticationMethod: "custom",
                            claims: GetUserClaims(user));

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        public static Claim[] GetUserClaims(User user)
        {
            return new Claim[]
            {
                new Claim("user_id", user.Id ?? ""),
                new Claim(JwtClaimTypes.Name,
                    (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                        ? (user.FirstName + " " + user.LastName)
                        : ""),
                new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.LastName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),

                //roles
                new Claim(JwtClaimTypes.Role, "user")
            };
        }

        public class ProfileService : IProfileService
        {
            //services
            private readonly IUser _userRepository;

            public ProfileService(IUser userRepository)
            {
                _userRepository = userRepository;
            }

            //Get user profile date in terms of claims when calling /connect/userinfo
            public async Task GetProfileDataAsync(ProfileDataRequestContext context)
            {
                try
                {
                    //depending on the scope accessing the user data.
                    if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                    {
                        //get user from db (in my case this is by email)
                        var user = await _userRepository.GetUserByEmail(context.Subject.Identity.Name);

                        if (user != null)
                        {
                            var claims = GetUserClaims(user);

                            //set issued claims to return
                            context.IssuedClaims =
                                claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                    else
                    {
                        //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                        //where and subject was set to my user id.
                        var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                        if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                        {
                            //get user from db (find user by user id)
                            var user = await _userRepository.GetUserByEmail(userId.Value);

                            // issue the claims for the user
                            if (user != null)
                            {
                                var claims = GetUserClaims(user);

                                context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type))
                                    .ToList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //log your error
                }
            }

            //check if user account is active.
            public async Task IsActiveAsync(IsActiveContext context)
            {
                try
                {
                    //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                       
                        var user = await _userRepository.GetById(userId.Value);


                        context.IsActive = user.EmailConfirmed;
                    }
                }
                catch (Exception ex)
                {
                    //handle error logging
                }
            }
        }
    }
}