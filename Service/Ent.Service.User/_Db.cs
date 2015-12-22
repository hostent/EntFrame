using Ent.DataAccess;
using Ent.Model.Responsity;
using Ent.Model.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Service.User
{
    public class _Db
    {
        /// <summary>
        /// 用户模块，数据仓储,Mongo
        /// </summary>
        public static IResponsity MongoDb
        {
            get
            {
                return (IResponsity)new MongoResponsity(MongoContext.Db, "U");
            }
        }

        /// <summary>
        /// 用户模块，数据仓储 ,sql
        /// </summary>
        public static IResponsity SqlDb
        {
            get
            {
                return (IResponsity)new SqlResponsity<UserDBContext>(new UserDBContext());
            }
        }



    }
}
