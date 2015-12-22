using Ent.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ent.Common.Mvc
{
    public class GridResult<T> : JsonResult
    {
        DataPage<T> Data { get; set; }

        string Template { get; set; }

        public GridResult(DataPage<T> data, string template)
        {
            Data = data;
            Template = template;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            string str = context.Controller.ControllerContext.RenderViewToString(Template, Data.CurrentPage);

            var obj = new { TotalCount = Data.TotalCounts, CurrentPageHtml = str };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            context.HttpContext.Response.Write(json);
            context.HttpContext.Response.ContentType = "application/json";

            context.HttpContext.Response.Flush();
        }
    }

    public class ListResult<T> : JsonResult
    {
        IList<T> Data { get; set; }

        string Template { get; set; }

        public ListResult(IList<T> data, string template)
        {
            Data = data;
            Template = template;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            string str = context.Controller.ControllerContext.RenderViewToString(Template, Data);

            var obj = new { CurrentPageHtml = str };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            context.HttpContext.Response.Write(json);
            context.HttpContext.Response.ContentType = "application/json";

            context.HttpContext.Response.Flush();
        }
    }
}
