using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common
{
    public class CookieHelp
    {
        public static void SetCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Domain = System.Configuration.ConfigurationSettings.AppSettings["domain"];
            cookie.Path = "/";
            cookie.HttpOnly = true;
            cookie.Value = value;

            HttpContext.Current.Response.SetCookie(cookie);

        }
        public static void SetCookie(string key, string value, DateTime Expires)
        {
            var cookie = new System.Web.HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = Expires;
            SetCookie(cookie);

        }
        public static void SetCookie(HttpCookie cookie)
        {
            cookie.HttpOnly = true;
            cookie.Path = "/";
            cookie.Domain = System.Configuration.ConfigurationSettings.AppSettings["domain"];
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static HttpCookie GetCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null)
            {
                return null;
            }
            return cookie;
        }
        public static string GetCookieValue(string key)
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null)
            {
                return string.Empty;
            }
            return cookie.Value;
        }

        public static void RemoveCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                cookie.Domain = System.Configuration.ConfigurationSettings.AppSettings["domain"];
                cookie.Expires = DateTime.Now.AddDays(-1);//设置Cookie失效
                HttpContext.Current.Response.Cookies.Add(cookie);

                HttpContext.Current.Request.Cookies.Remove(key);//移除当前Cookie
            }
        }
    }
}
