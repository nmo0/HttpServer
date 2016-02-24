using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VISTEN.HTTPServer {
    public class Log {
        public DateTime DateTime { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }
        public string Message { get; set; }
    }
}
