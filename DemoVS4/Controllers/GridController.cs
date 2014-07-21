using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data.Linq;
using DemoVS4.Core.Manager;

namespace DemoVS4.Controllers
{
    public class GridController : Controller
    {
        //
        // GET: /Grid/
        DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
        DemoVS4.Core.Manager.ProductManager PM = new ProductManager();
        public ActionResult Inline()
        {
            return View();
        }

        public ActionResult PopUp()
        {
            return View();
        }

        public JsonResult GetJsonOutputForGridDataSelect()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();

            var ListWitoutFK = List.Select(x => new
            {
                ProductID=x.ProductID,
                ProductName=x.ProductName,
                UniqueCode=x.UniqueCode,
                UnitPrice=x.UnitPrice,
                UnitsInStock=x.UnitsInStock,
                Discontinued=x.Discontinued,
                Category=x.Category,
                CreatedDate=x.CreatedDateTime,
                Duration=2
            });

            return Json(ListWitoutFK);  
        }

        public JsonResult GetJsonOutputForGridDataUpdatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration=20 };
            }
            temp.ProductID = PM.insertUpdate(temp);
            temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
            return Json(temp);
        }

        public JsonResult GetJsonOutputForGridDataDeletePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate };
            }
            PM.DeleteRecordById(temp.ProductID);
            return Json(new { value = "success" });
        }

        public JsonResult GetJsonOutputForGridDataCreatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = 0, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration=10 };
            }
            temp.ProductID = PM.insertUpdate(temp);
            temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
            
            return Json(temp);
        }

        public JsonResult GetJsonOutputForFoodUniqueCategory()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = (from c in ctx.Products.ToList()
                                    group c by c.Category into d
                                    select new
                                    {
                                        Category = d.Key,
                                    }); //groupby Food by category
            return Json(publicationTable);
        }
        public JsonResult SearchFOODbyCategory(string searchString)
        {
            //searchString = searchString.ToUpper();
            var searchedFOOD = from m in ctx.Products.ToList()
                               where m.Category.ToUpper().Contains(searchString.ToUpper()) || m.ProductName.ToUpper().Contains(searchString.ToUpper()) || m.UnitsInStock.ToString().ToUpper().Contains(searchString.ToUpper()) || m.UnitPrice.ToString().ToUpper().Contains(searchString.ToUpper())
                               select m;

            //return new JavaScriptSerializer().Serialize(searchedFOOD.ToList()); //if return type is string
            return Json(searchedFOOD);
        }


    }


}
