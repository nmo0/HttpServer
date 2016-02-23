using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace VISTEN.HTTPServer {
    public class SocketTool {

        public delegate void SocketHandle(Socket socket);
        public string Host { get; set; }
        public int Port { get; set; }

        public SocketHandle RequestSocket;

        public SocketTool(string host, int port) {
            this.Host = host;
            this.Port = port;
        }

        public SocketTool(string host, int port, SocketHandle requestSocket) {
            this.Host = host;
            this.Port = port;
            this.RequestSocket = requestSocket;
        }

        public void Start() {
            IPAddress ip = IPAddress.Parse(this.Host);
            IPEndPoint ipEnd = new IPEndPoint(ip, this.Port);
            //服务器流程
            // 1.创建新的套接字 socket
            Console.WriteLine("创建新的套接字...");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2.将套接字绑定到80端口 bind
            socket.Bind(ipEnd);

            // 3.允许套接字进行连接 listen
            Console.WriteLine("开始监听...");
            socket.Listen(0);
            // 4.等待连接 accept
            while (true) {
                try {
                    // 5.(3)通知应用程序有连接到来(4)
                    Socket socketTemp = socket.Accept();
                    //转发请求
                    RequestSocket(socketTemp);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }
            // 9.关闭close
            socket.Close();
        }
    }
}
