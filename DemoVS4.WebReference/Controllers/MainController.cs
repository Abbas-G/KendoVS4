using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;
using System.Data.Linq;
using DemoVS4.Core.Manager;
using System.Data;

namespace DemoVS4.WebReference.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/
        DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
        DemoVS4.Core.Manager.ProductManager PM = new ProductManager();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WebService()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetJsonOutputFromController()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();

            var ListWitoutFK = List.Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Discontinued = x.Discontinued,
                Category = x.Category,
                CreatedDate = x.CreatedDateTime,
                Duration = 2
            });

            return Json(ListWitoutFK, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetJsonOutputFromWebService()
        {
            localhost.ProductWebService webService = new localhost.ProductWebService();

            DemoVS4.WebReference.localhost.DridDataObj[] WebServiceItems = webService.ReadProduct("aaa");

            //Saop header value set
            localhost.AuthHeader authentication = new  localhost.AuthHeader();
            authentication.Password = "test";
            webService.AuthHeaderValue = authentication;
            //


            var ListWitoutFK = WebServiceItems.Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Discontinued = x.Discontinued,
                Category = x.Category,
                CreatedDate = x.CreatedDate,
                Duration = 2
            });

            return Json(ListWitoutFK, JsonRequestBehavior.AllowGet);
        }

    }
}
