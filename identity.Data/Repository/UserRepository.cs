
using System.Threading.Tasks;
using identity.Data.Abstraction;
using identity.Data.ExceptionHandler;
using identity.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace identity.Data.Repository
{
  public  class UserRepository:IUser
    {
        private readonly IdentityContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(IdentityContext context,UserManager<User> userManager,SignInManager<User> signInManager )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<User> Login(User user, string password)
        {
          var   result = await _signInManager.CheckPasswordSignInAsync(user, password,false);
            if (!result.Succeeded)
            {
                return null;
            }
             return user;

        }
        
        public async Task<User> Register(User user, string password)
        {
            user.UserName = user.Email;
          var result = await _userManager.CreateAsync(user, password);
          if (result.Succeeded)
          {
              await _signInManager.SignInAsync(user, true);
          }
           return user;
        }

        public async Task<User> GetById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> VerifyEmailExits(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
    }
}
