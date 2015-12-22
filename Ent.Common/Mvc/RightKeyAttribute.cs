using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.Mvc
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RightKeyAttribute : Attribute
    {
        public string Key { get; set; }

        public RightKeyAttribute()
        {

        }

        public RightKeyAttribute(string key)
        {
            Key = key;
        }

    }
}
