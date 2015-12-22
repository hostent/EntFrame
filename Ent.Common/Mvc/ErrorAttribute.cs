using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    /// <summary>
    /// 错误异常过滤器
    /// </summary>
    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
    }
}