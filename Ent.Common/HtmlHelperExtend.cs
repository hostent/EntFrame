using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtend
    {
        public static string Url(this HtmlHelper helper, string part)
        {
            string root = System.Configuration.ConfigurationSettings.AppSettings["www.rootUrl"].ToString();
            return root + part;
        }

        public static string RenderViewToString(this ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            context.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context,
                                  viewResult.View,
                                  context.Controller.ViewData,
                                  context.Controller.TempData,
                                  sw);
                try
                {
                    viewResult.View.Render(viewContext, sw);
                }
                catch (Exception ex)
                {
                    throw;
                }

                return sw.GetStringBuilder().ToString();
            }
        }

        public static void RenderWebParts(this HtmlHelper helper, string partName, object model)
        {
            RenderPartialExtensions.RenderPartial(helper, "~/bin/WebParts/" + partName + ".cshtml", model);
        }

        #region Resource


        public static string Version
        {
            get
            {
                return "1.1";
            }
        }

        public static bool IsCompress
        {
            get
            {
                return false;
            }
        }

        public static IHtmlString Css(this HtmlHelper helper, string targetUrl, params string[] vpath)
        {
            if (IsCompress)
            {
                //todo
            }

            string template = "<link rel=\"stylesheet\" href=\"{0}\" />";

            StringBuilder sb = new StringBuilder();

            foreach (var item in vpath)
            {
                string url = Ent.Common.RootUrlConfig.ResourceUrl + item + "?v=" + Version;
                sb.Append("\r\n");
                sb.Append(string.Format(template, url));
            }

            return helper.Raw(sb.ToString());

        }


        public static IHtmlString Js(this HtmlHelper helper, string targetUrl, params string[] vpath)
        {
            if (IsCompress)
            {
                //todo
            }

            string template = "<script src=\"{0}\"></script>";

            StringBuilder sb = new StringBuilder();

            foreach (var item in vpath)
            {
                string url = Ent.Common.RootUrlConfig.ResourceUrl + item + "?v=" + Version;
                sb.Append("\r\n");
                sb.Append(string.Format(template, url));
            }

            return helper.Raw(sb.ToString());

        }

        #endregion
    }


}
