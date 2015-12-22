using Ent.Common;
using Ent.Common.Mvc;
using Ent.Model.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aijia.User.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult Login(string userName, string password)
        {
           
           
            var result = UContainer.GetInstance<ICurrentUser>().Login(userName, password, this.UserKey);

            if (result.Tag == 1)
            {
                UserKeyFilter.UpdateUserKey((string)result.Data);
            }

            return Json(result);
        }


    }
}