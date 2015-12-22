using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{
    public class RequestHelp
    {
        public static string GetHead(string key)
        {
            if(System.Web.HttpContext.Current!=null)
            {
                return System.Web.HttpContext.Current.Request.Headers.Get(key);
            }
            return null;
        }
    }
}
