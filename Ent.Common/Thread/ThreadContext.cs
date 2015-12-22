using Ent.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common.Thread
{
    public class ThreadContext
    {

        public static string ThreadAddress
        {
            get
            {
                if (string.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name))
                {
                    System.Threading.Thread.CurrentThread.Name = Guid.NewGuid().ToString("N");
                }
                return System.Threading.Thread.CurrentThread.Name;
            }
        }
 

        public static bool RunningState
        {
            get
            {
                string key = ThreadAddress + ".RunningState";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return false;
                }
                return (bool)Cache.CacheHelp.Default.Get(key); 
            }
            set
            {
                string key = ThreadAddress + ".RunningState";
                Cache.CacheHelp.Default.Add(key, value);
            }
        }


        /// <summary>
        /// 事务标识
        /// </summary>
        public static string TranTag
        {
            get
            {
                string key = ThreadAddress + ".TranTag";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return string.Empty;
                }
                return (string)Cache.CacheHelp.Default.Get(key); 
            }
            set
            {
                string key = ThreadAddress + ".TranTag";
                Cache.CacheHelp.Default.Add(key, value);
            }
        }

        /// <summary>
        /// 当前数据库操作上下文
        /// </summary>
        public static IDictionary<string, dynamic> DbContexts
        {
            get
            {
                string key = ThreadAddress + ".DbContexts";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    Cache.CacheHelp.Default.Add(key, new Dictionary<string, dynamic>());
                }
                return (IDictionary<string, dynamic>)Cache.CacheHelp.Default.Get(key);
            }
        }

        /// <summary>
        /// 线程进度百分比，最大100，最小 0
        /// </summary>
        public static decimal Percentage
        {
            get
            {
                string key = ThreadAddress + ".Percentage";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return 0;
                }
                return (decimal)Cache.CacheHelp.Default.Get(key); 
            }
            set
            {
                string key = ThreadAddress + ".Percentage";

                Cache.CacheHelp.Default.Add(key, value);
            }

        }

        /// <summary>
        /// 线程运行时状态消息
        /// </summary>
        public static string Message
        {
            get
            {
                string key = ThreadAddress + ".Message";
                var cache = Cache.CacheHelp.Default.Get(key);
                if (cache == null)
                {
                    return "";
                }
                return (string)Cache.CacheHelp.Default.Get(key); 
            }
            set
            {
                string key = ThreadAddress + ".Message";
                Cache.CacheHelp.Default.Add(key, value);
            }
        }

        public static void ReleaseContext()
        {
            Cache.CacheHelp.Default.Remove(ThreadAddress + ".TranTag");
            Cache.CacheHelp.Default.Remove(ThreadAddress + ".DbContexts");
            Cache.CacheHelp.Default.Remove(ThreadAddress + ".Percentage");
            Cache.CacheHelp.Default.Remove(ThreadAddress + ".Message");
        }





    }

}
