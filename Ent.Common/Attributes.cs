using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common
{

    /// <summary>
    /// 开放api
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiAttribute : Attribute
    {
        public ApiAttribute()
        {
            Level = ApiLevel.Application;
        }
        public ApiAttribute(ApiLevel level)
        {
            Level = level;
        }

        public ApiLevel Level { get; set; }


    }

    public enum ApiLevel
    {
        Application,
        Session
    }
}
