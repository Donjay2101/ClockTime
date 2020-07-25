using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.ResponseViewModels
{
    public class BasicResponse
    {
        public int StatusCode { get; set; }
        public object Result { get; set; }
    }
}
