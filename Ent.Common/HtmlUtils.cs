using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common
{
    public class HtmlUtils
    {
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }

        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        public static string UrlEncode(string text)
        {
            return HttpUtility.UrlEncode(text);
        }

        public static string UrlDecode(string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        /// <summary>
        /// 数字id 变成字符串
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static String BlurIdentify(Object identify, String flag = "et")
        {
            if (identify == null) throw new ArgumentNullException();
            if (String.IsNullOrWhiteSpace(identify.ToString()))
                throw new ArgumentException("Identify value is empty string.");

            identify = identify.ToString().Replace("-", String.Empty).Replace(",", String.Empty).Replace(".", String.Empty);

            Int32 convertedIdentify;
            String blurredString = String.Empty;
            var random = new Random(DateTime.Now.Millisecond);

            if (Int32.TryParse(identify.ToString(), out convertedIdentify))
            {
                blurredString += flag;

                foreach (char item in convertedIdentify.ToString(CultureInfo.InvariantCulture))
                {
                    blurredString += char.ConvertFromUtf32(97 + 7 + Int32.Parse(item.ToString(CultureInfo.InvariantCulture)));
                    blurredString += item.ToString(CultureInfo.InvariantCulture);
                }

                return blurredString;
            }

            throw new InvalidCastException("Identify cannot cast to blurred text.");
        }
        /// <summary>
        /// 字符串解出数字id
        /// </summary>
        /// <param name="blurredIdentify"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static Int32 UnblurIdentify(Object blurredIdentify, String flag = "et")
        {
            if (blurredIdentify == null) throw new ArgumentNullException();
            if (String.IsNullOrWhiteSpace(blurredIdentify.ToString()))
                throw new ArgumentException("Blurred identify value is empty string.");

            String tempString = String.Empty;

            foreach (char item in blurredIdentify.ToString())
                if (item >= 48 && item <= 57) tempString += item.ToString(CultureInfo.InvariantCulture);

            Int32 result;
            if (Int32.TryParse(tempString, out result)) return result;

          

            return -1;
        }

        public static string GetIp()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }
            return System.Web.HttpContext.Current.Request.UserHostAddress;
        }
        /// <summary>
        /// 获取完整的url
        /// </summary>
        /// <returns></returns>
        public static string GetIntactRequestUrl()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }
            return System.Web.HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 获取当前请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrl()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }
            return System.Web.HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 获取当前网站的根url
        /// </summary>
        /// <returns>当前网站的根url</returns>
        public static string GetRootUrl()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }
            return System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }
    }
}
