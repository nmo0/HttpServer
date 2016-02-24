using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace VISTEN.HTTPServer {
    public class SocketTool {

        public delegate void SocketHandle(Socket socket);
        public string Host { get; set; }
        public int Port { get; set; }

        private Socket socket;

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
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 2.将套接字绑定到80端口 bind
            socket.Bind(ipEnd);

            // 3.允许套接字进行连接 listen
            Console.WriteLine("开始监听...");
            socket.Listen(100);

            new Thread(OnStart).Start();
        }

        private void OnStart() {

            // 4.等待连接 accept
            while (true) {
                try {
                    // 5.(3)通知应用程序有连接到来(4)
                    Socket socketTemp = socket.Accept();
                    Console.WriteLine(string.Format("端口{0}收到请求,加入线程队列。。",Port));
                    //接收到请求后，将请求放到线程队列中
                    ThreadPool.QueueUserWorkItem(AcceptSocket,socketTemp);
                    //转发请求
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

            }
            // 9.关闭close
            //socket.Close();
        }

        private void AcceptSocket(object temp) {
            Socket socketTemp = temp as Socket;
            Console.WriteLine(string.Format("端口{0}开始处理。。", Port));
            RequestSocket(socketTemp);
        }
    }
}
