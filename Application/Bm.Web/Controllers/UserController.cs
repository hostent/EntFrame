using Bm.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bm.Web.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Default1/
        public ActionResult Index()
        {
            
           

            return View();
        }

        #region 用户登陆注册
        public ActionResult Reg()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        #endregion


        #region 密码找回

        public ActionResult FindPwd()
        {
            return View();
        }

        #endregion
    }
}