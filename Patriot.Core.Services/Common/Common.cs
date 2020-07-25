using Patriot.Core.Services.ResponseViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.Common
{
    public static class Common
    {
        public static string GenerateID()
        {             
            var guid = Guid.NewGuid().ToString();
            return guid.Replace("-", "");
        }


        public static BasicResponse SendResponse(int statusCode, string message)
        {
            return new BasicResponse { Result = message, StatusCode = statusCode };
        }
    }
}
