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
                byte[] recvBytes;
                int bytes = 0;
                int bytesLength = 0;
                //bytes = _socket.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                //_socket.Receive(recvBytes, 1024, 0);

                //从缓冲区循环读取数据
                do {
                    recvBytes = new byte[1024];
                    bytes = _socket.Receive(recvBytes, recvBytes.Length, 0);

                    bytesLength += bytes;

                    data += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                } while (bytes >= 1024);

                if (bytesLength <= 0) {
                    Console.WriteLine("发送响应。。");
                    SendResponse(400, new byte[0], new Dictionary<string,string>(),false);
                    return;
                }

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


                STSdb4Log.Info(request.Url);

                if (request.Method == "OPTIONS") {

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    //response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add("Allow", "GET, POST, TRACE, OPTIONS");
                    Console.WriteLine("发送响应。。");
                    SendResponse(200, new byte[0], response.Headers,false);

                } else if (request.Method == "TRACE") {

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    //response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add("Content-Type", "text/html;charset=utf-8");
                    Console.WriteLine("发送响应。。");
                    SendResponse(200, Encoding.UTF8.GetBytes(request.ToString()), response.Headers,false);

                } else {
                    //这里要判断是否静态文件
                    //校验请求url是否合法
                    //TODO:暂时不作处理
                    //转法请求

                    _host.ProcessRequest(this, request);
                }

                #endregion

            } catch (Exception e) {
                STSdb4Log.Info(e.Message);
                //Console.WriteLine("解析报文失败 Error:{0}",e.Message);
            }
        }

        public void Close() {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void SendResponse(byte[] data) {
            _socket.Send(data);
        }

        public void SendResponse(int statusCode, byte[] responseBodyBytes, Dictionary<string, string> headers, bool keepAlive) {
            //发送Header时需保持连接知道Body发送完毕
            SendHeaders(statusCode, headers, responseBodyBytes.Length, keepAlive);
            if(responseBodyBytes.Length>0)
                _socket.Send(responseBodyBytes);

            if (!keepAlive)
                Close();
        }

        public void SendHeaders(int statusCode, Dictionary<string, string> headers,int contentLength, bool keepAlive) {
            var responseStr = new HttpResponseMessage() {
                Headers = headers,
                StatusCode = statusCode
            }.HeadersToString(contentLength, keepAlive);

            _socket.Send(Encoding.UTF8.GetBytes(responseStr));
        }


        /// <summary>
        ///  对象“/***/***.rem”已经断开连接或不在服务器上。
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService() {
            //Remoting对象 无限生存期
            return null;
        }
    }
}
