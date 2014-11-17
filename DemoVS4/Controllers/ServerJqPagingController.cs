using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoVS4.Controllers
{
    public class ServerJqPagingController : Controller
    {
        //
        // GET: /ServerJqPaging/
        private static int pagesize = 3;
        private const string startFolder = @"D:\Workspace\RND\extras 2,3,4\workspace\KendoVS4\DemoVS4\Content\kendo\peoples";
        public ActionResult bootpag()
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            ViewData["Total"] = (from file in fileList select file).Count();
            ViewData["PageSize"] = pagesize;
            return View();
        }

        [HttpPost]
        public ActionResult getPage(int page, int total)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fullfileinfo =
                (from file in fileList select file).Skip(page-1*pagesize).Take(pagesize);

            return PartialView("InnerView", fullfileinfo);
        }

    }

}
