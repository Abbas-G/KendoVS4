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
        DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
        DemoVS4.Core.Manager.ProductManager PM = new DemoVS4.Core.Manager.ProductManager();
        public ActionResult bootpag()
        {
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();
            ViewData["Total"] = ctx.Products.ToList().Count();
            ViewData["PageSize"] = pagesize;
           // ViewData["PageNumber"] = (int)Math.Ceiling((double)ViewData["Total"] / (double)pagesize);
            ViewData["PageNumber"] = ((int)ViewData["Total"] + pagesize -1)/ pagesize;
            return View();
        }

        [HttpPost]
        public ActionResult getPage(int page)
        {

            //List<DemoVS4.Core.DAL.Product> items =
            //    ctx.Products.ToList().Skip(page*pagesize).Take(pagesize);

            List<DemoVS4.Core.DAL.Product> items = (from v in ctx.Products.Skip((page - 1) * pagesize)
                                .Take(pagesize)
                        select v).ToList();

            return PartialView("InnerView", items);
        }

    }

}
