
using Ent.Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aijia.User.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Customer/Default/

        [AuthUser]
        public ActionResult Index()
        {
            return View();
        }
    }
}