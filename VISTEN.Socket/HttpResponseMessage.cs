using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer
{
    public class HttpResponseMessage:HttpMessage
    {
        public string Version { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; set; }

    }
}
