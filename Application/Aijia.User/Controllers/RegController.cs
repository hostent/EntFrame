using Ent.Common;
using Ent.Model.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aijia.User.Controllers
{
    public class RegController : BaseController
    {
        //
        // GET: /Reg/
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult HasRegedit(string phoneNum)
        {

            var result = UContainer.GetInstance<IReg>().HasRegedit(phoneNum);

            return Json(result);
        }
	}
}