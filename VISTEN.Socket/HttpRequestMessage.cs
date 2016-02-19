using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer
{
    public class HttpRequestMessage:HttpMessage
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Version { get; set; }
    }
}
