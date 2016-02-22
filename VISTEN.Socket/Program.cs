using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace VISTEN.HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Func<string, string> func = (string data) => {
            //    return data;
            //};

            SocketTool httpServer = new SocketTool("127.0.0.1", 8090, delegate(string data){
                if (string.IsNullOrEmpty(data))
                    return string.Empty;

                // 7.处理HTTP请求报文(6)

                #region 解析HTTP报文

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


                //Console.WriteLine(string.Format("request address is{0}", request.StartLine));

                #endregion

                Console.WriteLine("回送HTTP响应...");
                string sendStr = string.Empty;

                if (request.Method == "OPTIONS") {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add("Allow", "GET, POST, TRACE, OPTIONS");
                    response.Headers.Add("Content-Length", "0");
                    sendStr = response.ToString();
                } else if (request.Method == "TRACE") {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add(HttpResponseHeader.Date.ToString(), DateTime.Now.ToString("r"));
                    response.Headers.Add("Content-Type", "text/html;charset=utf-8");
                    response.Headers.Add("Content-Length", request.ToString().Length.ToString());
                    response.Body = request.ToString();
                    sendStr = response.ToString();
                } else {
                    string resultStr = "I am accept your message!";

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Headers = new Dictionary<string, string>();
                    response.StartLine = "HTTP/1.1 200 OK";
                    response.Headers.Add(HttpResponseHeader.Date.ToString(), DateTime.Now.ToString("r"));
                    response.Headers.Add("Content-Type", "text/html;charset=utf-8");
                    response.Headers.Add("Content-Length", resultStr.Length.ToString());
                    response.Body = resultStr;
                    sendStr = response.ToString();
                }
                return sendStr;
            });

            httpServer.Start();
            //int port = 8090;
            //string host = "127.0.0.1";
            //IPAddress ip = IPAddress.Parse(host);
            //IPEndPoint ipEnd = new IPEndPoint(ip, port);
            ////服务器流程
            //// 1.创建新的套接字 socket
            //Console.WriteLine("创建新的套接字...");
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //// 2.将套接字绑定到80端口 bind
            //socket.Bind(ipEnd);

            //// 3.允许套接字进行连接 listen
            //Console.WriteLine("开始监听...");
            //socket.Listen(0);
            //// 4.等待连接 accept
            //while (true)
            //{
            //    try
            //    {
            //        #region 操作
            //        // 5.(3)通知应用程序有连接到来(4)
            //        Socket socketTemp = socket.Accept();

            //        // 6.开始读取请求read(5)
            //        string recvStr = "";
            //        byte[] recvBytes = new byte[1024];
            //        int bytes;
            //        bytes = socketTemp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
            //        recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);

            //        if (string.IsNullOrEmpty(recvStr))
            //            continue;

            //        // 7.处理HTTP请求报文(6)

            //        #region 解析HTTP报文

            //        Console.WriteLine("解析HTTP报文...");

            //        HttpRequestMessage request = new HttpRequestMessage();
            //        request.Headers = new Dictionary<string, string>();

            //        string[] requestMessages = recvStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //        if (requestMessages.Length <= 0)
            //            requestMessages = recvStr.Split(new string[] { "\r" }, StringSplitOptions.None);
            //        if (requestMessages.Length > 0)
            //        {
            //            //解析起始行
            //            request.StartLine = requestMessages[0];
            //            var startLineStrs = request.StartLine.Split(' ');
            //            request.Method = startLineStrs[0];
            //            request.Url = startLineStrs[1];
            //            request.Version = startLineStrs[2];


            //            for (int i = 1; i < requestMessages.Length; i++)
            //            {
            //                var str = requestMessages[i];
            //                if (string.IsNullOrEmpty(str))
            //                {
            //                    //Header 结束
            //                    request.Body = requestMessages[i + 1];
            //                    break;
            //                }
            //                //读取Header
            //                var kTemp = str.Remove(str.IndexOf(':'));
            //                var vTemp = str.Substring(str.IndexOf(':') + 1);
            //                request.Headers.Add(kTemp, vTemp.Trim());
            //            }
            //        }


            //        //Console.WriteLine(string.Format("request address is{0}", request.StartLine));

            //        #endregion
                    
            //        Console.WriteLine("回送HTTP响应...");
            //        string sendStr = string.Empty;

            //        if (request.Method == "OPTIONS")
            //        {
            //            HttpResponseMessage response = new HttpResponseMessage();
            //            response.Headers = new Dictionary<string, string>();
            //            response.StartLine = "HTTP/1.1 200 OK";
            //            response.Headers.Add("Allow", "GET, POST, TRACE, OPTIONS");
            //            response.Headers.Add("Content-Length", "0");
            //            sendStr = response.ToString();
            //        }
            //        else if (request.Method == "TRACE")
            //        {
            //            HttpResponseMessage response = new HttpResponseMessage();
            //            response.Headers = new Dictionary<string, string>();
            //            response.StartLine = "HTTP/1.1 200 OK";
            //            response.Headers.Add(HttpResponseHeader.Date.ToString(), DateTime.Now.ToString("r"));
            //            response.Headers.Add("Content-Type", "text/html;charset=utf-8");
            //            response.Headers.Add("Content-Length", request.ToString().Length.ToString());
            //            response.Body = request.ToString();
            //            sendStr = response.ToString();
            //        }
            //        else
            //        {
            //            string resultStr = "I am accept your message!";

            //            HttpResponseMessage response = new HttpResponseMessage();
            //            response.Headers = new Dictionary<string, string>();
            //            response.StartLine = "HTTP/1.1 200 OK";
            //            response.Headers.Add(HttpResponseHeader.Date.ToString(), DateTime.Now.ToString("r"));
            //            response.Headers.Add("Content-Type", "text/html;charset=utf-8");
            //            response.Headers.Add("Content-Length", resultStr.Length.ToString());
            //            response.Body = resultStr;
            //            sendStr = response.ToString();
            //        }

            //        // 8.回送HTTP响应 write(7)


            //        byte[] bs = Encoding.ASCII.GetBytes(sendStr);
            //        socketTemp.Send(bs, bs.Length, 0);//返回信息给客户端
            //        socketTemp.Close();
            //        #endregion
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }

            //}
            //// 9.关闭close
            //socket.Close();
        }
    }
}
