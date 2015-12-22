using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{
    public class AppConfig
    {
        public static bool IsDebug()
        {
            string tag = System.Configuration.ConfigurationSettings.AppSettings["Config.IsDebug"] ?? "false";

            return Convert.ToBoolean(tag);
        }

        public static bool IsOpen()
        {
            string tag = System.Configuration.ConfigurationSettings.AppSettings["Function.IsOpen"] ?? "true";

            return Convert.ToBoolean(tag);
        }

        public static bool IsDebugLog()
        {
            string tag = System.Configuration.ConfigurationSettings.AppSettings["Log.IsDebug"] ?? "true";

            return Convert.ToBoolean(tag);
        }
    }

    public class RootUrlConfig
    {
        /// <summary>
        /// 共赢社（PC）,这个是旧的php 地址，到时候会改掉，新版的不要用这个 用 p2p 的那个地址
        /// </summary>
        public static string CowinclubUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://devcms.cowinclub.cn:8000";
                }
                return "http://old.cowinclub.cn";
            }
        }

        /// <summary>
        /// 共赢社（M），这个是旧的php 地址，到时候会改掉，新版的不要用这个 用 p2p 的那个地址 + M
        /// </summary>
        public static string MCowinclubUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://m.devcms.cowinclub.cn:8010";
                }
                return "http://m.cowinclub.cn";
            }
        }

        /// <summary>
        /// 体验金
        /// </summary>
        public static string ExperienceCashUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://tyj.devcms.cowinclub.cn";
                }
                return "http://tyj.cowinclub.cn";
            }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static string CustomerUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://customer.devcms.cowinclub.cn";
                }
                return "http://customer.cowinclub.cn";
            }
        }

        public static string P2pUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://p2p.devcms.cowinclub.cn";
                }
                return "http://www.cowinclub.cn";
            }
        }

        
        public static string IntUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://int.devcms.cowinclub.cn";
                }
                return "http://int.cowinclub.cn";
            }
        }

        /// <summary>
        /// 月月赢url地址
        /// </summary>
        public static string YyyUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://yyy.devcms.cowinclub.cn";
                }
                return "http://yyy.cowinclub.cn";
            }
        }

        /// <summary>
        /// 共赢财富url地址
        /// </summary>
        public static string GycfUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://gycf.devcms.cowinclub.cn";
                }
                return "http://gycf.cowinclub.cn";
            }
        }
 

        public static string ResourceUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://resource.devcms.cowinclub.cn";
                }
                return "http://resource.cowinclub.cn";
            }
        }

        /// <summary>
        /// 共赢社旧版（PC）
        /// </summary>
        public static string OldCowinclubUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://devcms.cowinclub.cn:8000";
                }
                return "http://old.cowinclub.cn";
            }
        }

        /// <summary>
        /// 赢计划url地址
        /// </summary>
        public static string YjhUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://yjh.devcms.cowinclub.cn";
                }
                return "http://yjh.cowinclub.cn";
            }
        }


        public static string ActiveTqUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式
                    return "http://tq.devcms.cowinclub.cn";
                }
                return "http://tq.cowinclub.cn";
            }
        }
        /// <summary>
        /// 联名卡
        /// </summary>
        public static string ShopUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                    return "http://shop.devcms.cowinclub.cn";
                return "http://shop.cowinclub.cn";
            }
        }

        public static string MidWebUrl
        {
            get
            {
                if (AppConfig.IsDebug())
                {//调试模式 
                    return "http://mid.devcms.cowinclub.cn";
                }
                return "http://mid.cowinclub.cn";
            }
        }
    }
}
