using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
  public class NotFoundResponse<T>:BaseResponse<T>
    {
        public NotFoundResponse(string message, T body)
        {
            Message = message;
            Body = body;
            Success = false;
            Code = 404;
        }
    }
}
