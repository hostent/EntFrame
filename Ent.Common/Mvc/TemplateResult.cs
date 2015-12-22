using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    public class TemplateResult : ContentResult
    {
        object Data { get; set; }

        string Template { get; set; }


        public TemplateResult(object t, string template)
        {
            Data = t;
            Template = template;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            string str = context.Controller.ControllerContext.RenderViewToString(Template, Data);

            context.HttpContext.Response.Write(str);
            context.HttpContext.Response.ContentType = "text/plain";

            context.HttpContext.Response.Flush();
        }
    }
    public class TemplateResult<T> : ContentResult
    {
        T Data { get; set; }

        string Template { get; set; }


        public TemplateResult(T t, string template)
        {
            Data = t;
            Template = template;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            string str = context.Controller.ControllerContext.RenderViewToString(Template, Data);

            context.HttpContext.Response.Write(str);
            context.HttpContext.Response.ContentType = "text/plain";

            context.HttpContext.Response.Flush();
        }
    }
}
