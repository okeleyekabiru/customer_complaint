using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using identity.Data.Abstraction;
using identity.Data.Model;
using IdentityModel;
using IdentityServer4.Models;
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
                    var contextPassword = new PasswordHasher<User>().HashPassword(user, context.Password);
                    if (user.PasswordHash == contextPassword)
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
    }
}