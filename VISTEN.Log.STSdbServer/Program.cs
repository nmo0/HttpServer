using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Reflection;
using STSdb4.Database;

namespace VISTEN.Log.STSdbServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + "\\Log\\";
            
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            using (IStorageEngine engine = STSdb.FromFile(path + "log.stsdb4")) {
                var server = STSdb.CreateServer(engine, 7182);
                server.Start();
                Console.WriteLine("Server is running");
                while (true) {
                    Console.WriteLine("Press 'exit' to stop server");
                    var accept = Console.ReadLine();
                    if (accept == "exit") {
                        server.Stop();
                        return;
                    }
                }
            }
        }


    }
}
