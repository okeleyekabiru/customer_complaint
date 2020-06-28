using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
   public class BadResponse<T>:BaseResponse<T>
    {
        public BadResponse(string message, T body)
        {
            Code = 400;
            Success = false;
            Message = message;
            Body = body;
        }

        
    }
}
