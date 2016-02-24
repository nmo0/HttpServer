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
            //STSdb4Log.Info(string.Format("请求转发到网站"));
            Console.WriteLine("请求转发到网站管道。。");
            CoHttpWorkerRequest workerRequest = new CoHttpWorkerRequest(this, processor, requestInfo);
            HttpRuntime.ProcessRequest(workerRequest);
        }
    }
}
