using Ent.Common;
using Ent.Common.Mvc;
using Ent.DataAccess;
using Ent.Model.Entity.Context;
using Ent.Model.Responsity;
using Ent.Model.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aijia.User
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
                return UContainer.GetInstance<ICurrentUser>().Get(this.UserKey);
            }

        }


        public IQuery QuerySqlUser
        {
            get
            {
                return (IResponsity)new SqlResponsity<UserDBContext>(new UserDBContext());
            }
        }

    }
}