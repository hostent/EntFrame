using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.LinqHelp
{
    public class Entity
    {
        public static string Column<T>(Expression<Func<T, object>> exp)
        {
            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(exp.Body);

            return "{"+conditionBuilder.Condition+"}";
        }
    }
}
