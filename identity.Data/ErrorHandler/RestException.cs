using System;
using System.Collections.Generic;
using System.Text;

namespace identity.Data.ErrorHandler
{
  public  class RestException:Exception
    {
        const  string Message ="An Error Occured While Resolving Host";
        public RestException():base(Message)
        {
            
        }

        public RestException(Exception innerException):base(Message,innerException)
        {
            
        }

        public RestException(string message, Exception innerException):base(message,innerException)
        {
            
        }

        public RestException(string message):base(message)
        {
            
        }
    }
}
