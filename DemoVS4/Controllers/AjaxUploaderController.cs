using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DemoVS4.Controllers
{
    public class AjaxUploaderController : Controller
    {
        //
        // GET: /AjaxUploader/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void AjaxForm(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Verify that the user selected a file
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the fielname
                        var fileName = Path.GetFileName(file.FileName);
                        // TODO: need to define destination
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        file.SaveAs(path);
                    }
                }
            }
        }

    }
}
