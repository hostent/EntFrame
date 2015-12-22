using Ent.Common;
using Ent.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    public class UserKeyFilter : ActionFilterAttribute
    {
        public const string USERKEY = "user-key";

        public static string GetUserKey()
        {
            string result = "";
            var userKeyCookie = CookieHelp.GetCookie(UserKeyFilter.USERKEY);
            if (userKeyCookie == null)
            {
                result = Guid.NewGuid().ToString("N");
                CookieHelp.SetCookie("user-key", result, DateTime.Now.AddMonths(1));
            }
            else
            {
                result = userKeyCookie.Value;
            }
            return HtmlUtils.UrlDecode(result);
        }

        public static string GetUserKey(string userId)
        {
            string result = AESHelper.AESEncrypt(userId);

            return result;
        }

        public static string GetUserId(string userKey)
        {
            return AESHelper.AESDecrypt(userKey);
        }

        public static void UpdateUserKey(string userKey)
        {
            CookieHelp.SetCookie(USERKEY, HtmlUtils.UrlEncode(userKey), DateTime.Now.AddMonths(1));

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (CookieHelp.GetCookie(USERKEY) == null)
            {
                CookieHelp.SetCookie(USERKEY, Guid.NewGuid().ToString("N"), DateTime.Now.AddMonths(1));
            }
        }
    }
}