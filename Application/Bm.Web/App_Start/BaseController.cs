using Ent.Common;
using Ent.Common.Mvc;
using Ent.DataAccess;
using Ent.Model.Entity.Context;
using Ent.Model.Responsity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bm.Web.App_Start
{
    /// <summary>
    /// 基础类
    /// </summary>
    public class BaseController : Controller
    {
        public string UserKey
        {
            get
            {
                return UserKeyFilter.GetUserKey();
            }
        }

        public UserContext CurrentUser
        {
            get
            {
                return null;
            }


        }


        public IQuery QuerySqlUser
        {
            get
            {
                return (IQuery)new SqlResponsity(new SqlDbContext.UserDBContext());
            }
        }

    }
}