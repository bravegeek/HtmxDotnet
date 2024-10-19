using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmxDotnet.utils
{
    public static class HTMXUtils
    {
        public static bool IsHtmxRequest(this HttpRequest request)
        {
            return request.Headers.ContainsKey("HX-Request");
        }
    }
}