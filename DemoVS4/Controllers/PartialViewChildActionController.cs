using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoVS4.Controllers
{
    public class PartialViewChildActionController : Controller
    {
        //
        // GET: /PartialViewChildAction/

        public ActionResult Index()
        {
            return View();
        }
        //[ChildActionOnly] //childactiononly is used by page action cant used by ajax
        [ActionName("childajax")]
        public ActionResult Child(string msg)
        {
            ViewData["Msg"] = msg;
            return PartialView("child");
        }


    }
}
