using Ent.DataAccess;
using Ent.Model.Responsity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Service.InviteBids
{
    public class _Db
    {
        /// <summary>
        /// 招标模块，数据仓储
        /// </summary>
        public static IResponsity MongoDb
        {
            get
            {
                return (IResponsity)new MongoResponsity(MongoContext.Db, "I");
            }
        }
    }
}
