using System;
using System.Collections.Generic;
using System.Text;

namespace Response.Model
{
   public class BaseResponse<T>
    {
        public static string Message { get; set; }
        public static bool Success { get; set; }
        public static int Code { get; set; }
        public static T Body { get; set; }

       
    }
}
