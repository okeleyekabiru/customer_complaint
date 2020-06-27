using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;

namespace identity.Data.ExceptionHandler
{
   public class UserNotFoundException:Exception
   {
       private const string Message = "user you are looking for does not exist";

       public UserNotFoundException():base(Message)
       {
           
       }

       public UserNotFoundException(Exception innerException):base(Message,innerException)
       {
           
       }

       public UserNotFoundException(string message , Exception exception):base(message,exception)
       {
           
       }

       public UserNotFoundException(string message):base(message)
       {
           
       }

   }
}
