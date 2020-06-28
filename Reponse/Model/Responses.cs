using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
   public  class Responses<T>
    {
        public static OkResponse<T> OkResponse(string message, T body) {
            return new OkResponse<T>(message, body);

        }
        public static BadResponse<T> BadResponse(string message, T body)
        {
            return new BadResponse<T>(
               message,body
            );
        }
        public static InternalServerResponse<T> InternalServerResponsee(string message, T body)
        {
            return  new InternalServerResponse<T>(message,body);
        }
        public static NotFoundResponse<T> NotFoundResponse(string message, T body)
        {
            return new NotFoundResponse<T>(message,body);
        }

    }
}
