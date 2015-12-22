using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.Cache
{
    public class CacheHelp
    {
        public static ICache Default
        {
            get
            {
                return new RuntimeCache();
            }

        }
    }
}
