using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using STSdb4.Database;
using System.Threading;


namespace VISTEN.HTTPServer {
    public class STSdb4Log {

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(Log log) {
            ThreadPool.QueueUserWorkItem(SendMessage,log);
        }

        private static  void SendMessage(object log) {
            using (IStorageEngine engine = STSdb.FromNetwork("localhost", 7182)){
                var table = engine.OpenXTable<string, Log>("table");
                table.InsertOrIgnore(System.Guid.NewGuid().ToString(), log as Log);
                engine.Commit();
            }
        }

        /// <summary>
        /// 有问题。暂时屏蔽
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        internal static void Info(string message,string url = "") {
            //Info(new Log() {
            //    Message = message,
            //    Url = url,
            //    DateTime = DateTime.Now
            //});
        }
    }
}
