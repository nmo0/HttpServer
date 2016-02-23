using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace VISTEN.HTTPServer {
    /// <summary>
    /// HTTP类
    /// 实例化的时候，将接收到的Socket转发到这里
    /// </summary>
    public class HttpProcessor : MarshalByRefObject {

        private Socket _socket;
        private CoHost _host;

        public HttpProcessor(CoHost host, Socket socket) {
            _host = host;
            _socket = socket;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessRequest() {
            try {


                #region 解析HTTP报文
                
                string data = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = _socket.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息

                if (bytes <= 0) {
                    SendResponse(400, new byte[0], null);
                    return;
                }

                data += Encoding.ASCII.GetString(recvBytes, 0, bytes);


                Console.WriteLine("解析HTTP报文...");

                HttpRequestMessage request = new HttpRequestMessage();
                request.Headers = new Dictionary<string, string>();

                string[] requestMessages = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (requestMessages.Length <= 0)
                    requestMessages = data.Split(new string[] { "\r" }, StringSplitOptions.None);
                if (requestMessages.Length > 0) {
                    //解析起始行
                    request.StartLine = requestMessages[0];
                    var startLineStrs = request.StartLine.Split(' ');
                    request.Method = startLineStrs[0];
                    request.Url = startLineStrs[1];
                    request.Version = startLineStrs[2];


                    for (int i = 1; i < requestMessages.Length; i++) {
                        var str = requestMessages[i];
                        if (string.IsNullOrEmpty(str)) {
                            //Header 结束
                            request.Body = requestMessages[i + 1];
                            break;
                        }
                        //读取Header
                        var kTemp = str.Remove(str.IndexOf(':'));
                        var vTemp = str.Substring(str.IndexOf(':') + 1);
                        request.Headers.Add(kTemp, vTemp.Trim());
                    }
                }
                #endregion

                #region 处理请求类型

                if (request.Method == "OPTIONS") {

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add("Allow", "GET, POST, TRACE, OPTIONS");
                    SendResponse(200, new byte[0], response.Headers);

                } else if (request.Method == "TRACE") {

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add("Content-Type", "text/html;charset=utf-8");
                    SendResponse(200, Encoding.UTF8.GetBytes(request.ToString()), response.Headers);

                } else {
                    //这里要判断是否静态文件
                    //校验请求url是否合法
                    //TODO:暂时不作处理
                    //转法请求

                    _host.ProcessRequest(this, request);
                }

                #endregion



            } catch (Exception) {
                
                throw;
            }
        }

        public void Close() {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void SendResponse(int statusCode, byte[] responseBodyBytes, Dictionary<string, string> headers = null, bool keepAlive = false) {
            SendHeaders(statusCode, headers, responseBodyBytes.Length, keepAlive);
            _socket.Send(responseBodyBytes);

            if (!keepAlive)
                Close();
        }

        public void SendHeaders(int statusCode, Dictionary<string, string> headers,int contentLength, bool keepAlive = true) {

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("HTTP/1.1 {0} OK\r\n", statusCode);
            builder.AppendFormat("Date: {0}\r\n",
                                 DateTime.Now.ToUniversalTime().ToString("R"));
            if (contentLength > 0)
                builder.AppendFormat("Content-Length: {0}\r\n", contentLength);
            if (keepAlive)
                builder.Append("Connection: keep-alive\r\n");
            if (headers != null) {
                foreach (KeyValuePair<string, string> pair in headers) {
                    builder.AppendFormat("{0}: {1}\r\n", pair.Key, pair.Value);
                }
            }
            builder.Append("\r\n");

            _socket.Send(Encoding.UTF8.GetBytes(builder.ToString()));
        }

        public void SendResponse(byte[] data) {
            _socket.Send(data);
        }
    }
}
