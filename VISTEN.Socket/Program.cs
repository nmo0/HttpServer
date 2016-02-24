using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace VISTEN.HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Func<string, string> func = (string data) => {
            //    return data;
            //};

            //服务器路径 /port:端口号 /path:网站物理路径 /vpath:网站虚拟路径
            //WebDev.WebServer.EXE /port:80 /path:"c:\mysite" /vpath:"/"

            var keys = System.Configuration.ConfigurationManager.AppSettings.Keys;
            foreach (var item in keys) {

                Console.WriteLine("正在启动网站：0.0.0.0:{0}",item);
                STSdb4Log.Info(string.Format("正在启动网站：0.0.0.0:{0}", item));
                string dir = System.Configuration.ConfigurationManager.AppSettings[item.ToString()];

                InitHostFile(dir);

                CoHost host = (CoHost)ApplicationHost.CreateApplicationHost(typeof(CoHost), "/", dir);

                new SocketTool("127.0.0.1", Convert.ToInt32(item.ToString()), delegate(Socket socket) {

                    // 7.处理HTTP请求报文(6)

                    //请求转发给ASP.NET 运行时处理
                    HttpProcessor processor = new HttpProcessor(host, socket);
                    processor.ProcessRequest();

                }).Start();
            }

            //string dir = @"C:\Users\user\Documents\visual studio 2010\Projects\WebDemo\WebDemo";
            //InitHostFile(dir);
            //
            //CoHost host = (CoHost)ApplicationHost.CreateApplicationHost(typeof(CoHost), "/", dir);
            //
            //
            //new SocketTool("127.0.0.1", 8091, delegate(Socket socket){
            //
            //    // 7.处理HTTP请求报文(6)
            //
            //    //请求转发给ASP.NET 运行时处理
            //    HttpProcessor processor = new HttpProcessor(host, socket);
            //    processor.ProcessRequest();
            //
            //}).Start();
        }


        /// <summary>
        /// 代码拷贝、创建ASP.NET应用程序域
        /// </summary>
        /// <param name="dir"></param>
        private static void InitHostFile(string dir) {
            string path = Path.Combine(dir, "bin");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string source = Assembly.GetExecutingAssembly().Location;
            string target = path + "/" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            if (File.Exists(target))
                File.Delete(target);
            File.Copy(source, target);
        }
    }
}
