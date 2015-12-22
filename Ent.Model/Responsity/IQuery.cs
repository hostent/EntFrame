
using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Responsity
{
    public interface IQuery
    {
        IQueryable<T> Query<T>() where T : BaseTable;


        #region sql xml

        IList<T> Query<T>(string reportName, IDictionary<string, object> par);

        DataPage<T> QueryPage<T>(string reportName, PagePars param) where T : IPageModel;

        #endregion
    }
}
