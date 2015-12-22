using Ent.Model.Untility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Model.Responsity
{
    public interface IResponsity : IQuery,ITran
    {
        T Add<T>(T t) where T : BaseTable;

        IList<T> Add<T>(IList<T> list) where T : BaseTable;


        int Delete<T>(string id) where T : BaseTable, new();

        int Delete<T>(IList<string> ids) where T : BaseTable, new();


        int Update<T>(T t) where T : BaseTable, new();

        int Update<T>(IList<T> list) where T : BaseTable, new();


       
    }
}
