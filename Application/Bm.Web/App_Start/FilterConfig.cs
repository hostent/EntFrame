using Ent.Common.Mvc;
using System.Web;
using System.Web.Mvc;

namespace Bm.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorAttribute());

            filters.Add(new UserKeyFilter());
        }
    }
}
