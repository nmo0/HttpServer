using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer {
    [Serializable]
    public class HttpRequestMessage:HttpMessage
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Version { get; set; }

        public override string ToString() {
            StringBuilder tempStr = new StringBuilder();

            base.StartLine = Method + " " + Url + " " + Version;

            tempStr.Append(base.StartLine);
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
