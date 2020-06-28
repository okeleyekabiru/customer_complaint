using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
   public class InternalServerResponse<T>:BaseResponse<T>
    {
        public InternalServerResponse(string message,T body)
        {
            Code = 500;
            Success = false;
            Message = message;
            Body = body;
        }
    }
}
