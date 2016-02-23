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
        public string StatusCode { get; set; }
        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; set; }

        public override string ToString() {
            StringBuilder tempStr = new StringBuilder();

            base.StartLine = Version + " " + StatusCode + " " + ReasonPhrase;

            tempStr.Append(StartLine);
            tempStr.Append("\r\n");
            foreach (var item in Headers) {
                tempStr.Append(item.Key);
                tempStr.Append(": ");
                tempStr.Append(item.Value);
                tempStr.Append("\r\n");
            }
            tempStr.Append("\r\n");
            tempStr.Append(Body);
            return tempStr.ToString();
        }
    }
}
