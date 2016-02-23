using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VISTEN.HTTPServer {
    /// <summary>
    /// 应用程序域
    /// </summary>
    public class CoHost:MarshalByRefObject {

        public string PhysicalDir { get; private set; }

        public string VituralDir { get; private set; }

        public void Config(string vitrualDir, string physicalDir) {
            VituralDir = vitrualDir;
            PhysicalDir = physicalDir;
        }

        public void ProcessRequest(HttpProcessor processor, HttpRequestMessage requestInfo) {
            CoHttpWorkerRequest workerRequest = new CoHttpWorkerRequest(this, processor, requestInfo);
            HttpRuntime.ProcessRequest(workerRequest);
        }
    }
}
