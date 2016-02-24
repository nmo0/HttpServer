using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using STSdb4.Database;


namespace VISTEN.HTTPServer {
    public class STSdb4Log {

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message,string host = "0.0.0.0",int port = 0,string url = "") {

            //var path = Directory.GetCurrentDirectory() + "\\Log\\";
            //
            //if (!Directory.Exists(path)) {
            //    Directory.CreateDirectory(path);
            //}
            //
            //using (IStorageEngine engine = STSdb.FromFile(path + DateTime.Now.ToString("yyyyMMdd") + ".stsdb4")) {
            //    var table = engine.OpenXTable<string, Log>("table");
            //    table.InsertOrIgnore(System.Guid.NewGuid().ToString(), new Log() {
            //        Message = message,
            //        DateTime = DateTime.Now,
            //        Url = url,
            //        Port = port,
            //        Host = host
            //    });
            //    engine.Commit();
            //}
        }
    }
}
