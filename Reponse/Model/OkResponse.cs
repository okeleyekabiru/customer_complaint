using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
   public class OkResponse<T>:BaseResponse<T>
    {
        public OkResponse(string message, T body)
        {
            Code = 200;
            Success = true;
            Message = message;
            Body = body;
        }
       
    }
}
