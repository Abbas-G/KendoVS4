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

        public ActionResult Mix()
        {
            //reference link http://stackoverflow.com/questions/25451176/kendo-grid-can-you-add-row-with-a-popup-and-edit-inline
            return View();
        }

        [JsonpFilter]
        public ActionResult Foo() //cross domain check using jsonp attribute
        {
            var Data = new { Prop1 = "Abbas", Prop2 = "Galiyakotwala" };
            return Json(Data, JsonRequestBehavior.AllowGet); 
        }
        #region use below client code in cross domain
        //$.getJSON('http://localhost:3373/grid/foo?callback=?', function (data) {
        //    alert(data.Prop1);
        //});
        #endregion

        public ActionResult GridFromModel()
        {
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();
            var serializer = new JavaScriptSerializer();
            var ListWitoutFK = List.Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Discontinued = x.Discontinued,
                Category = x.Category
            });
            ViewData["JsonData"] = serializer.Serialize(ListWitoutFK);
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

            return Json(ListWitoutFK,JsonRequestBehavior.AllowGet);  
        }

        public JsonResult GetJsonOutputForGridDataUpdatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration = 2};
            }
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
                temp = new DridDataObj { ProductID = 0, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration = 10 };
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

        public JsonResult CheckDuplication(string ProductName) {
            DemoVS4.Core.DAL.Product items = ctx.Products.Where(x => x.ProductName == ProductName).FirstOrDefault();
            if (items!=null)
                return Json(new { value = "true" });
            else
                return Json(new { value = "false" });
             

            /*JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = ctx.Products.OrderBy(x=>x.ProductName);
            //return Json(publicationTable, JsonRequestBehavior.AllowGet);  //this statement allow to see records directly on browser , you need also to set get in place of post in javascript(.aspx page)
            return Json(publicationTable);  // this statement wont allow to see records directly on browser*/
        }


        public JsonResult JsonOutput()
        {
            Parent ob = new Parent();
            List<Sub> objsub = new List<Sub>();
            objsub.Add(new Sub { securityCode = "CBA"});
            objsub.Add(new Sub { securityCode = "BHP"});

            ob.securityCodes = objsub;

            if (Request.HttpMethod.ToUpperInvariant() == "GET")
                return Json(ob,JsonRequestBehavior.AllowGet);
            else
                return Json(ob);            
        }
    }

    public class Parent
    {
        public List<Sub> securityCodes { get; set; }
    }

    public class Sub
    {
        public string securityCode;
    }
}

#region Attribute define To return jsonp data
public class JsonpFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(
            ActionExecutedContext filterContext)
    {
        if (filterContext == null)
            throw new ArgumentNullException("filterContext");

        //
        // see if this request included a "callback" querystring
        // parameter
        //
        string callback = filterContext.HttpContext.
                  Request.QueryString["callback"];
        if (callback != null && callback.Length > 0)
        {
            //
            // ensure that the result is a "JsonResult"
            //
            JsonResult result = filterContext.Result as JsonResult;
            if (result == null)
            {
                throw new InvalidOperationException(
                    "JsonpFilterAttribute must be applied only " +
                    "on controllers and actions that return a " +
                    "JsonResult object.");
            }

            filterContext.Result = new JsonpResult
            {
                ContentEncoding = result.ContentEncoding,
                ContentType = result.ContentType,
                Data = result.Data,
                Callback = callback
            };
        }
    }
}
public class JsonpResult : JsonResult
{
    /// <summary>
    /// Gets or sets the javascript callback function that is
    /// to be invoked in the resulting script output.
    /// </summary>
    /// <value>The callback function name.</value>
    public string Callback { get; set; }

    /// <summary>
    /// Enables processing of the result of an action method by a
    /// custom type that inherits from
    /// <see cref="T:System.Web.Mvc.ActionResult"/>.
    /// </summary>
    /// <param name="context">The context within which the
    /// result is executed.</param>
    public override void ExecuteResult(ControllerContext context)
    {
        if (context == null)
            throw new ArgumentNullException("context");

        HttpResponseBase response = context.HttpContext.Response;
        if (!String.IsNullOrEmpty(ContentType))
            response.ContentType = ContentType;
        else
            response.ContentType = "application/javascript";

        if (ContentEncoding != null)
            response.ContentEncoding = ContentEncoding;

        if (Callback == null || Callback.Length == 0)
        {
            Callback = context.HttpContext.
              Request.QueryString["callback"];
        }

        if (Data != null)
        {
            // The JavaScriptSerializer type was marked as obsolete
            // prior to .NET Framework 3.5 SP1 
#pragma warning disable 0618
            JavaScriptSerializer serializer =
                 new JavaScriptSerializer();
            string ser = serializer.Serialize(Data);
            response.Write(Callback + "(" + ser + ");");
#pragma warning restore 0618
        }
    }
}

#endregion