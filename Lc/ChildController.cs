using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestLaneCove.Controllers
{
    public class ChildController : Controller
    {
        //
        // GET: /Child/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection result)
        {
            int flag = 0;
            int i = 1;

            foreach (var key in result.Keys)
            {
                if (key.ToString().StartsWith("RName"))
                {
                    string RName = !string.IsNullOrEmpty(result["RName" + i]) ? result["RName" + i].Trim() : "";
                    if (RName != "")
                    {
                        string RBrief = !string.IsNullOrEmpty(result["RBrief" + i]) ? result["RBrief" + i].Trim() : "";
                        if (RBrief != "")
                        {
                            flag = 1;
                        }
                    }

                    i++;
                }
            }

            if (flag == 1)
                ViewData["OnSuccess"] = "Records Inserted Succesfully";

            return View();
        }


        public ActionResult AddRows()
        {
            return View();
        }
    }
}
