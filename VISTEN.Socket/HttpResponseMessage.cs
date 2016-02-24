using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer {
    [Serializable]
    public class HttpResponseMessage:HttpMessage
    {
        public string Version { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; set; }

        public string HeadersToString(int contentLength = -1, bool keepAlive = false) {
            StringBuilder tempStr = new StringBuilder();

            tempStr.AppendFormat("HTTP/1.1 {0} {1}\r\n", StatusCode, "OK");
            tempStr.AppendFormat("Date: {0}\r\n", DateTime.Now.ToUniversalTime().ToString("R"));
            if (contentLength > 0)
                tempStr.AppendFormat("Content-Length: {0}\r\n", contentLength);
            if (keepAlive)
                tempStr.Append("Connection: keep-alive\r\n");
            foreach (var item in Headers)
                tempStr.AppendFormat("{0}: {1}\r\n", item.Key, item.Value);
            tempStr.Append("\r\n");
            return tempStr.ToString();
        }

        public string ToString(int contentLength = -1, bool keepAlive = false) {
            StringBuilder tempStr = new StringBuilder();
            tempStr.Append(HeadersToString(contentLength, keepAlive));
            tempStr.Append(Body);
            return tempStr.ToString();
        }
    }
}
