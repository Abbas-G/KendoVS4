using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data.Linq;
using DemoVS4.KendoGridUtilities;
using DemoVS4.Core.Manager;

namespace DemoVS4.Controllers
{
    public class ServerGridDetailController : Controller
    {
        //
        // GET: /ServerGridDetail/
        DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
        DemoVS4.Core.Manager.ProductManager PM = new ProductManager();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetJsonOutputForGridDataSelect(int? skip, int? take, int? page, int? pageSize, string group)
        {
            DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
            System.Data.Common.DbTransaction transaction = ctx.Connection.BeginTransaction();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            var sorterCollection = KendoGridSorterCollection.BuildCollection(Request);
            var filterCollection = KendoGridFilterCollection.BuildCollection(Request);

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
                CreatedDate = x.CreatedDateTime
            });

            var filteredItems = ListWitoutFK.MultipleFilter(filterCollection.Filters);
            var sortedItems = filteredItems.MultipleSort(sorterCollection.Sorters).ToList();
            var count = sortedItems.Count();

            if (page.HasValue)
            {
                var data = (from v in sortedItems.Skip((page.Value - 1) * pageSize.Value)
                                .Take(pageSize.Value)
                            select v).ToList();

                return Json(
                new
                {
                    File = data,
                    TotalCount = count
                },
                JsonRequestBehavior.AllowGet);
            }
            else {
                var data = (from v in sortedItems.Skip((1 - 1) * 5)
                                .Take(5)
                            select v).ToList();

                return Json(
                new
                {
                    File = data,
                    TotalCount = count
                },
                JsonRequestBehavior.AllowGet);
            
            }

            
        }

        public JsonResult GetJsonOutputForGridDataUpdatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            DridDataObj m = new JavaScriptSerializer().Deserialize<DridDataObj>(models);
            DridDataObj temp = new DridDataObj();

                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate };

            temp.ProductID = PM.insertUpdate(temp);
            temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
            /*
            DemoVS4.Core.DAL.Product l = ctx.Products.Where(x => x.ProductID == 3).FirstOrDefault();
            if (l != null)
            {
                l.UnitPrice = 50;
                ctx.SubmitChanges();
            } 
            */
            return Json(temp);
        }

        public JsonResult GetJsonOutputForGridDataDeletePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            DridDataObj m = new JavaScriptSerializer().Deserialize<DridDataObj>(models);
            DridDataObj temp = new DridDataObj();

            temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate };

            PM.DeleteRecordById(temp.ProductID);
            return Json(new { value = "success" });
        }

        public JsonResult GetJsonOutputForGridDataCreatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            DridDataObj m = new JavaScriptSerializer().Deserialize<DridDataObj>(models);
            DridDataObj temp = new DridDataObj();

            temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate };

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

            var ListWitoutFK = searchedFOOD.Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UniqueCode = x.UniqueCode,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Discontinued = x.Discontinued,
                Category = x.Category,
                CreatedDate = x.CreatedDateTime.Value.ToString("MM/dd/yyyy"),
                Duration = 2
            });
            //return new JavaScriptSerializer().Serialize(searchedFOOD.ToList()); //if return type is string
            return Json(ListWitoutFK);
        }

        public JsonResult CheckDuplication(string ProductName)
        {
            DemoVS4.Core.DAL.Product items = ctx.Products.Where(x => x.ProductName == ProductName).FirstOrDefault();
            if (items != null)
                return Json(new { value = "true" });
            else
                return Json(new { value = "false" });


            /*JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = ctx.Products.OrderBy(x=>x.ProductName);
            //return Json(publicationTable, JsonRequestBehavior.AllowGet);  //this statement allow to see records directly on browser , you need also to set get in place of post in javascript(.aspx page)
            return Json(publicationTable);  // this statement wont allow to see records directly on browser*/
        }

    }

}

