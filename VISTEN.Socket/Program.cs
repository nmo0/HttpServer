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
            int port = 8090;
            string host = "127.0.0.1";
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipEnd = new IPEndPoint(ip, port);
            //服务器流程
            // 1.创建新的套接字 socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2.将套接字绑定到80端口 bind
            socket.Bind(ipEnd);

            // 3.允许套接字进行连接 listen
            socket.Listen(0);
            try
            {
                // 4.等待连接 accept
                while (true)
                {
                    // 5.(3)通知应用程序有连接到来(4)
                    Socket socketTemp = socket.Accept();

                    // 6.开始读取请求read(5)
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = socketTemp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                    recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);

                    // 7.处理HTTP请求报文(6)

                    #region 解析HTTP报文

                    string startLine = string.Empty;
                    string method = string.Empty;
                    string url = string.Empty;
                    string version = string.Empty;

                    Dictionary<string,string> headers = new Dictionary<string,string>();
                    string body = string.Empty;

                    string[] requestMessages = recvStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    if (requestMessages.Length <= 0)
                        requestMessages = recvStr.Split(new string[] { "\r" }, StringSplitOptions.None);
                    if (requestMessages.Length>0)
                    {
                        //解析起始行
                        startLine = requestMessages[0];
                        var startLineStrs = startLine.Split(' ');
                        method = startLineStrs[0];
                        url = startLineStrs[1];
                        version = startLineStrs[2];

                        for (int i = 1; i < requestMessages.Length; i++)
                        {
                            var str = requestMessages[i];
                            if (string.IsNullOrEmpty(str))
                            {
                                //Header 结束
                                body = requestMessages[i + 1];
                                break;
                            }
                            //读取Header
                            var kvTemp = str.Split(':');
                            headers.Add(kvTemp[0], kvTemp[1]);
                        }
                    }


                    Console.WriteLine(string.Format("request address is{0}",startLine));

                    #endregion


                    Console.Clear();
                    Console.WriteLine(recvStr);

                    // 8.回送HTTP响应 write(7)

                    string resultStr = "I am accept your message!";

                    string sendStr = @"
HTTP/1.1 200 OK
Date: " + DateTime.Now.ToUniversalTime() + @"
Content-Type: text/html;charset=utf-8
Content-Length: " + resultStr.Length + @"

" + resultStr;
                    byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                    socketTemp.Send(bs, bs.Length, 0);//返回信息给客户端
                    socketTemp.Close();
                }
                // 9.关闭close

            }
            catch (Exception)
            {
                socket.Close();
                throw;
            }
        }
    }
}
