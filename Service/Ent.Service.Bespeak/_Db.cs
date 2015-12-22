using Ent.DataAccess;
using Ent.Model.Responsity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Service.Bespeak
{
    public class _Db
    {
        /// <summary>
        /// 预约模块，数据仓储
        /// </summary>
        public static IResponsity MongoDb
        {
            get
            {
                return (IResponsity)new MongoResponsity(MongoContext.Db, "B");
            }
        }
    }
}
