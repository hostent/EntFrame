using Ent.Model.Table;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.DataAccess
{


    #region set

    /// <summary>
    /// 用户模块
    /// </summary>
    public class UserDBContext : BaseDBContext
    {
        public UserDBContext()
            : base("DBConn_MSSql")
        {
        }


        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<U_User> U_User { get; set; }



    }


 






    #endregion

}
