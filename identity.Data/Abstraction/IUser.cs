using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using identity.Data.Model;

namespace identity.Data.Abstraction
{
   public interface IUser
   {
       Task<User> Login(User user,string password);
       Task<User> Register(User user,string password);
       Task<User> GetById(string userId);
       Task<User> GetUserByEmail(string email);
       Task<bool> VerifyEmailExits(string email);
       Task<bool> Commit();

   }
}
