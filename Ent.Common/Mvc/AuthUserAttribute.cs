using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    /// <summary>
    /// 登陆过滤器
    /// </summary>
    public class AuthUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}