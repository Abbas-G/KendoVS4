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
    public class GridMultiDropdownController : Controller
    {
        //
        // GET: /GridMultiDropdown/
        DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
        DemoVS4.Core.Manager.ProductManager PM = new ProductManager();
        public ActionResult Index()
        {
            return View();
        }

        #region Mutliselect text value
        public ActionResult multiform() {
            return View();
        }
        [HttpPost]
        public ActionResult multiform(FormCollection result)
        {
            string cat = result["Category"].ToString();
            return JavaScript("document.getElementById('status').innerHTML = 'Success';");
        }
        public JsonResult GetJsonOutputItem()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = (from c in ctx.Products.ToList()
                                    group c by c.Category into d
                                    select new
                                    {
                                        Text = d.Key,
                                        value=d
                                    }); //groupby Food by category
            
            return Json(publicationTable);
        }
        #endregion


        public PartialViewResult AddNewForm()
        {
            DridDataObj temp = new DridDataObj();
            //temp.Category = "Men, Casual";
            return PartialView("AddNewForm", temp);
        }

        [HttpPost]
        public ActionResult AddNewForm(DemoVS4.Core.Manager.DridDataObj temp, FormCollection result)
        {
            try
            {
                temp.Category = result["Category"].ToString();
                temp.ProductName = result["ProductName"].ToString();
                temp.Discontinued = (result["Discontinued"].ToString() == "false") ? false : true; //Html.CheckBoxFor() is wired, its send two value on server if true i.e "true,false" and if fasle send one value i.e "false"
                temp.UnitsInStock = Convert.ToInt32(result["UnitsInStock"].ToString());
                temp.UnitPrice = Convert.ToInt32(result["UnitPrice"].ToString());
                temp.CreatedDate = Convert.ToDateTime(result["CreatedDate"].ToString());
                //ViewData["Msg"] = "Success";
                temp.ProductID = PM.insertUpdate(temp);
                temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
                //return Content("Success");

                return JavaScript("document.getElementById('status').innerHTML = 'Success';setTimeout(function(){$('#AddNew').data('kendoWindow').close();},1000)");
            }
            catch (Exception e) {
                return Content("Server Error!"); ;
            }
        }

        public JsonResult GetJsonOutputForGridDataSelect()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();

            var ListWitoutFK = List.Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UniqueCode = x.UniqueCode,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Discontinued = x.Discontinued,
                Category = x.Category.Split(',').ToList<string>(), // Catergory is an array of string
                CreatedDate = x.CreatedDateTime,
                Duration = 2
            });

            return Json(ListWitoutFK, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJsonOutputForGridDataUpdatePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<GridDataObj2> persons = new JavaScriptSerializer().Deserialize<IList<GridDataObj2>>(models);
            DridDataObj temp = new DridDataObj();
            GridDataObj2 temp2 = new GridDataObj2();
            foreach (GridDataObj2 m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = string.Join(", ", m.Category), CreatedDate = m.CreatedDate, Duration = 2 };
                temp2 = new GridDataObj2 { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration = 2 };
            }
            temp.ProductID = PM.insertUpdate(temp);
            temp2.ProductID = temp.ProductID;
            temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
            temp2.UniqueCode = temp.UniqueCode;

            return Json(temp2);
        }

        public JsonResult GetJsonOutputForGridDataDeletePopup(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<GridDataObj2> persons = new JavaScriptSerializer().Deserialize<IList<GridDataObj2>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (GridDataObj2 m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = string.Join(", ", m.Category), CreatedDate = m.CreatedDate };
            }
            PM.DeleteRecordById(temp.ProductID);
            return Json(new { value = "success" });
        }

        //public JsonResult GetJsonOutputForGridDataCreatePopup(string models)
        //{
        //    JavaScriptSerializer jSerializer = new JavaScriptSerializer();
        //    IList<GridDataObj2> persons = new JavaScriptSerializer().Deserialize<IList<GridDataObj2>>(models);
        //    DridDataObj temp = new DridDataObj();
        //    foreach (GridDataObj2 m in persons)
        //    {
        //        temp = new DridDataObj { ProductID = 0, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = string.Join(", ", m.Category), CreatedDate = m.CreatedDate, Duration = 10 };
        //    }
        //    temp.ProductID = PM.insertUpdate(temp);
        //    temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);

        //    return Json(temp);
        //}

        public JsonResult GetJsonOutputForFoodUniqueCategory()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = (from c in ctx.Products.ToList()
                                    group c by c.Category into d
                                    select new
                                    {
                                        Category = d.Key,
                                    }); //groupby Food by category
            string[] ArrayOnlyfromlist= new string[publicationTable.ToList().Count];
            int i = 0;
            foreach (var m in publicationTable) {
                ArrayOnlyfromlist[i] = m.Category;
                i++;
            }
            return Json(ArrayOnlyfromlist);
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
                Category = x.Category.Split(',').ToList<string>(), // Catergory is an array of string
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
        }

    }
    public class GridDataObj2
    {
        public int? ProductID;
        public string ProductName;
        public string UniqueCode;
        public int UnitPrice;
        public int UnitsInStock;
        public Boolean Discontinued;
        public string[] Category; //array for multi select list
        public DateTime CreatedDate;
        public int Duration;
    }
}
