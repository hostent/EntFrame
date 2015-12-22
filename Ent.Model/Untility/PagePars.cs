using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Expressions;


namespace Ent.Model.Untility
{
    public class PagePars
    {
        public PagePars()
        {

            Where = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Where { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Order { get; set; }

    }

    public class PagePars<T>
    {
        public Expression<Func<T, bool>> Where { get; set; }

        public Expression<Func<T, object>>[] SortBys { get; set; }

        public Expression<Func<T, object>>[] SortByDescendings { get; set; }

        public int? Skip { get; set; }

        public int? Limit { get; set; }

    }

 


}
