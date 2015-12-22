using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.Log
{


    public class LogHelp
    {

        public enum LogSaveWay
        {
            Mongodb = 1
        }

        public static Func<string> OpUserID { get; set; }

        public static Func<string> OpUserName { get; set; }

        private static LogSaveWay? LogWay { get; set; }

        /// <summary>
        /// 进程启动的时候，配置下日志
        /// </summary>
        /// <param name="way"></param>
        /// <param name="OpUserID"></param>
        /// <param name="OpUserName"></param>
        public static void Config(LogSaveWay way, Func<string> opUserID = null, Func<string> opUserName = null)
        {
            OpUserID = opUserID;
            OpUserName = opUserName;
            LogWay = way;



        }

        public static ILog Default
        {
            get
            {
                if (LogWay == LogSaveWay.Mongodb || LogWay == null)
                {
                    var log = new MongoLog();

                    log.OpUserID = OpUserID == null ? "" : OpUserID();
                    log.OpUserName = OpUserName == null ? "" : OpUserName();

                    return log;
                }
                return null;
            }

        }
    }
}
