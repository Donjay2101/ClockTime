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
    }
}
