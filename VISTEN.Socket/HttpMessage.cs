using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer
{
    [Serializable]
    public class HttpMessage
    {
        public string StartLine { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            StringBuilder tempStr = new StringBuilder();
            tempStr.Append(StartLine);
            tempStr.Append("\r\n");
            foreach (var item in Headers)
            {
                tempStr.Append(item.Key);
                tempStr.Append(": ");
                tempStr.Append(item.Value);
                tempStr.Append("\r\n");
            }
            tempStr.Append("\r\n");
            tempStr.Append(Body);
            return tempStr.ToString();
        }

        /// <summary>
        /// 获取或设置Headers头属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name] {
            get {
                return Headers[name];
            }
            set {
                Headers[name] = value;
            }
        }
    }
}
