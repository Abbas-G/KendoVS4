using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace DemoVS4.Controllers
{
    public class TVPController : Controller
    {
        //
        // GET: /TVP/

        public ActionResult xml()
        {
            List<DemoVS4.Core.DAL.Product2> data = new List<DemoVS4.Core.DAL.Product2>{
            new DemoVS4.Core.DAL.Product2{ Unit=10,Name="TATA"},
            new DemoVS4.Core.DAL.Product2{ Unit=20,Name="Maruti"}
            };

            StringBuilder xmlString = new StringBuilder();

            xmlString.AppendFormat("<{0}>", "Products");
            foreach (var m in data)
            {
                xmlString.AppendFormat("<{0}>", "items");
                xmlString.AppendFormat("<Name>{0}</Name>", m.Name);
                xmlString.AppendFormat("<Unit>{0}</Unit>", m.Unit);
                xmlString.AppendFormat("</{0}>", "items");
            }
            xmlString.AppendFormat("</{0}>", "Products");

            DemoVS4.Core.Manager.Product2Manager PM2 = new Core.Manager.Product2Manager();
            int? msg = PM2.insertXML(xmlString.ToString());
            if (msg.Value >= 0)
                ViewData["Message"] = "Success";
            else
                ViewData["Message"] = "fail";
            return View();
        }

    }

}
