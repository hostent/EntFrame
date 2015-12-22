using Ent.Common.Security;
using Ent.Common.Thread;
using MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common.Log
{
    public class MongoLog : ILog
    {

        public string OpUserID { get; set; }

        public string OpUserName { get; set; }

        private string GetErrorMsg(Exception e)
        {
            string msg = "";

            Exception tag = e;
            for (int i = 0; i < 5; i++)
            {
                if (tag == null)
                {
                    break;
                }
                msg = msg + tag.Message;
                msg = msg + "\r\n ---------------------------堆栈信息------------------------------- \r\n ";
                msg = msg + tag.StackTrace;

                msg = msg + "\r\n --------------------------------------------------------------------------------------- \r\n ";
                tag = tag.InnerException;
            }           

            return msg;

        }

        private static LogLevel CurrentLevel
        {
            get
            {
                if (Convert.ToBoolean(AppConfig.IsDebugLog()))
                {
                    return LogLevel.Debug;
                }
                return LogLevel.Release;
            }
        }

        private void FillBase(BaseEntity entity)
        {
            HttpContext httpContext = HttpContext.Current;

            entity.AppPath = AppDomain.CurrentDomain.BaseDirectory;
            if (httpContext != null)
            {
                entity.Browser = httpContext.Request.Browser.Type + "," + httpContext.Request.Browser.Version;

                entity.UserId = OpUserID;
                entity.UserName = OpUserName;
                
                entity.UserIP = HtmlUtils.GetIp();
                entity.PageUrl = httpContext.Request.Url.ToString();
            }

            entity.Createtime = DateTime.Now;
            entity.MachineName = Dns.GetHostName();
            entity.ThreadName = ThreadContext.ThreadAddress;
            entity.Track = "";

        }     

        private void LogError(object errorEntity)
        {
            ErrorEntity entity = (ErrorEntity)errorEntity;
            Log(entity);
            
        }

        private void LogMessage(object messageEntity)
        {
            MessageEntity entity = (MessageEntity)messageEntity;
            Log(entity);
             
        }

        private void Log<T>(T t) where T : class
        {
            string connectionString = ConfigurationManager.AppSettings["mongoDbConfig"];
            if (String.IsNullOrWhiteSpace(connectionString)) connectionString = "Server=127.0.0.1:27017;MinimumPoolSize=10;MaximumPoolSize=500;Pooled=true";



            MongoClient client = new MongoClient(connectionString);          
            
            

            
            try
            {

                IMongoDatabase database = client.GetDatabase("MayanLogDb");

                var collection = database.GetCollection<T>(t.GetType().Name);

                collection.InsertOne(t);              
                 
            }
            catch
            {
                // Don't do anything.
            }
             
        }


        #region Entity

        enum LogLevel
        {
            Release,
            Debug
        }

        class ErrorEntity : BaseEntity
        {
            public string ErrorMessage { get; set; }

            public string StackTrace { get; set; }
        }

        class MessageEntity : BaseEntity
        {

            public string Category { get; set; }

            public string Message { get; set; }
            
        }

        class MessageEntity<T> : BaseEntity
        {

            public string Category { get; set; }

            public string Message { get; set; }

            public T CustomerMsg { get; set; }

        }

        class BaseEntity
        {
            public string UserId { get; set; }

            public string UserName { get; set; }

            public string UserIP { get; set; }

            public DateTime Createtime { get; set; }

            public string PageUrl { get; set; }

            public string Browser { get; set; }

            public string AppPath { get; set; }

            public string MachineName { get; set; }

            public string ThreadName { get; set; }

            public string Track { get; set; }
        }

        #endregion


        #region ILog

        public void Msg(string msg)
        {
            MessageEntity entity = new MessageEntity();
            FillBase(entity);
            entity.Category = "";
            entity.Message = msg;
           
            Asyn.Invork(() => {
                Log(entity);
            });

        }

        public void Msg(string category, string msg)
        {
            MessageEntity entity = new MessageEntity();
            FillBase(entity);

            entity.Category = category;
            entity.Message = msg;

            Asyn.Invork(() =>
            {
                Log(entity);
            });
        }

        public void DebugMsg(string msg)
        {
            if (CurrentLevel == LogLevel.Debug)
            {
                MessageEntity entity = new MessageEntity();
                FillBase(entity);
                entity.Message = msg;

                Asyn.Invork(() =>
                {
                    Log(entity);
                });
            }
        }


        public void Error(Exception ex)
        {
            ErrorEntity entity = new ErrorEntity();
            FillBase(entity);
            entity.ErrorMessage = GetErrorMsg(ex);

            Asyn.Invork(() =>
            {
                Log(entity);
            });

        }


        /// <summary>
        /// 自定义日志信息
        /// </summary>
        /// <typeparam name="T">日志信息类型</typeparam>
        /// <param name="msg">日志信息</param>
        public void Msg<T>(T msg) where T : class
        {
            MessageEntity<T> cMsg = new MessageEntity<T>();

            FillBase(cMsg);

            cMsg.CustomerMsg = msg;

            Asyn.Invork(() =>
            {
                Log(cMsg);
            });
        }

        #endregion
    }
}
