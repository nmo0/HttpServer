using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace VISTEN.HTTPServer {
    /// <summary>
    /// 自定义请求处理类
    /// </summary>
    class CoHttpWorkerRequest : HttpWorkerRequest {

        private CoHost coHost;
        private HttpProcessor processor;
        private HttpRequestMessage requestInfo;
        

        private int _statusCode;
        private Dictionary<string, string> _responseHeaders;
        private IList<byte[]> _responseBodyBytes;
        private string _statusDescription;


        private bool _isHeaderSent;

        public CoHttpWorkerRequest(CoHost coHost, HttpProcessor processor, HttpRequestMessage requestInfo) {
            // TODO: Complete member initialization
            this.coHost = coHost;
            this.processor = processor;
            this.requestInfo = requestInfo;


            _responseHeaders = new Dictionary<string, string>();
            _responseBodyBytes = new List<byte[]>();

        }
        public override void EndOfRequest() {
        }

        public override string GetHttpVerbName() {
            return requestInfo.Method;
        }

        public override string GetHttpVersion() {
            return requestInfo.Version;
        }

        public override string GetLocalAddress() {
            return string.Empty;
        }

        public override int GetLocalPort() {
            return 0;
        }

        public override string GetQueryString() {
            return string.Empty;
        }

        public override string GetRawUrl() {
            return requestInfo.Url;
        }

        public override string GetRemoteAddress() {
            var host = requestInfo.Headers.Where(m => m.Key == "Host").FirstOrDefault();
            return host.Value;
        }

        public override int GetRemotePort() {
            var host = requestInfo.Headers.Where(m => m.Key == "Host").FirstOrDefault().ToString();
            return 0;
        }

        public override string GetUriPath() {
            //var host = requestInfo.Headers.Where(m => m.Key == "Host").FirstOrDefault();
            return requestInfo.Url;
        }

        public override void SendKnownResponseHeader(int index, string value) {
            //throw new NotImplementedException();
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length) {
            //throw new NotImplementedException();
        }

        public override void SendResponseFromFile(string filename, long offset, long length) {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 从内存获取数据 发送响应
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public override void SendResponseFromMemory(byte[] data, int length) {
            //throw new NotImplementedException();
            if (length > 0)
            {
                byte[] dst = new byte[length];
                Buffer.BlockCopy(data, 0, dst, 0, length);
                _responseBodyBytes.Add(dst);
            }
        }

        public override void FlushResponse(bool finalFlush) {
            //发送响应
            if (!_isHeaderSent) {
                processor.SendHeaders( _statusCode, _responseHeaders, -1, finalFlush);
                _isHeaderSent = true;
            }
            for (int i = 0; i < _responseBodyBytes.Count; i++) {
                byte[] data = _responseBodyBytes[i];
                processor.SendResponse(data);
            }
            _responseBodyBytes = new List<byte[]>();
            if (finalFlush)
                processor.Close();
            Console.WriteLine("成功发送响应...");
        }

        public override void SendStatus(int statusCode, string statusDescription) {
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }

        public override void SendUnknownResponseHeader(string name, string value) {
            _responseHeaders[name] = value;
        }
    }
}
