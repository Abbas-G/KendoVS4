using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data.Linq;
using DemoVS4.KendoGridUtilities;
using DemoVS4.Core.Manager;
using System.IO;
using System.Web.Hosting;

namespace DemoVS4.Controllers
{
    public class ServerInlineController : Controller
    {
        //
        // GET: /ServerInline/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetJsonOutputForGridDataSelect(int skip, int take, int page, int pageSize, string group)
        {
            var sorterCollection = KendoGridSorterCollection.BuildCollection(Request);
            var filterCollection = KendoGridFilterCollection.BuildCollection(Request);

            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            List<ItemsObj> List = GetItems();
            var ListWitoutFK = List.Select(x => new
            {
                Id = x.Id,
                ImageUrl = !string.IsNullOrEmpty(x.ImageUrl) ? x.ImageUrl : "",
                DateS = x.DateS,
                Description = x.Description,
                url = x.url,
                isArabic = x.isArabic,
                Order = x.Order
            });

            var filteredItems = ListWitoutFK.MultipleFilter(filterCollection.Filters);
            var sortedItems = filteredItems.MultipleSort(sorterCollection.Sorters).ToList();
            var count = sortedItems.Count();
            var data = (from v in sortedItems.Skip((page - 1) * pageSize)
                            .Take(pageSize)
                        select v).ToList();

            return Json(
                new
                {
                    File = data,
                    TotalCount = count
                },
                JsonRequestBehavior.AllowGet); //alow get while using server grid
        }


        [HttpPost]
        public ActionResult editRow(string Id, string Description, string url, string DateS, string Order, string ImageUrl, bool isArabic)
        {
            try
            {
                ItemsObj data = new ItemsObj();
                data.Id = Convert.ToInt32(Id);
                data.ImageUrl = ImageUrl;
                data.DateS = DateS;
                data.Description =Description;
                data.url = url;
                data.isArabic = isArabic;
                data.Order = Convert.ToInt32(Order); 


                //if (NM.insertUpdate(data)>0)
                    return Json(new { Value = true });
               // else
                   // return Json(new { Value = false });
            }
            catch (Exception e)
            {
                return Json(new { Value = false });
            }
        }

        [HttpPost]
        public ActionResult DeleteRecords(int id)
        {
           // bool a = NM.DeleteRecordById(id);
            return Json(new { Value = "Id:" + id + " deleted Successfully" });
        }


        public JsonResult GetJsonOutputForitems()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();

            List<items> items = new List<items>();
            for (int i = 1; i <= 5; i++)
            {
                items item = new items();
                item.value = i;
                items.Add(item);
            }
            var publicationTable = items;
            return Json(publicationTable);
        }

        public JsonResult UploadFile(HttpPostedFileBase ImageUrl, int id)
        {
            string fileName2 = "News" + id.ToString() + Path.GetExtension(ImageUrl.FileName);
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/images/News"));
                foreach (var item in dirInfo.GetFiles())
                {
                    if (item.Name.Equals(fileName2))
                    {
                        String PreviousPath = dirInfo.FullName + @"\" + item.Name;
                        System.IO.File.Delete(PreviousPath);
                        break;
                    }
                }
            }
            catch (Exception e) { }

            var path = Path.Combine(Server.MapPath("~/Content/images/News"), fileName2);
            ImageUrl.SaveAs(path);

            return Json(new { ImageUrl = fileName2 });
            // return Json(new{data="Success","text/plain"});
        }

        public JsonResult UploadFile2(HttpPostedFileBase ImageUrl)
        {
            string fileName2 = "News" +Path.GetExtension(ImageUrl.FileName);
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/images/News"));
                foreach (var item in dirInfo.GetFiles())
                {
                    if (item.Name.Equals(fileName2))
                    {
                        String PreviousPath = dirInfo.FullName + @"\" + item.Name;
                        System.IO.File.Delete(PreviousPath);
                        break;
                    }
                }
            }
            catch (Exception e) { }

            var path = Path.Combine(Server.MapPath("~/Content/images/News"), fileName2);
            ImageUrl.SaveAs(path);

            return Json(new { ImageUrl = fileName2 });
            // return Json(new{data="Success","text/plain"});
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [NonAction]
        public List<ItemsObj> GetItems()
        {
            var list = new List<ItemsObj>{
            new ItemsObj{ Id=1, ImageUrl="x", DateS="20", Description="15",isArabic=true,url="Surati",Order=1},
            new ItemsObj{ Id=1, ImageUrl="x1", DateS="10", Description="115",isArabic=true,url="Surati",Order=2}
            };
            return list;
        }

        #region New Form
        public PartialViewResult AddNews()
        {
            ItemsObj data = new ItemsObj();
            data.isArabic = false;
            data.Order = 1;
            return PartialView("AddNews", data);
        }
        [HttpPost]
        public ActionResult AddNews(ItemsObj data, FormCollection result, HttpPostedFileBase ImageUrl)
        {
            try
            {
                data.Description = result["Description"].ToString();
                data.isArabic = (result["isArabic"].ToString() == "false") ? false : true; //Html.CheckBoxFor() is wired, its send two value on server if true i.e "true,false" and if fasle send one value i.e "false"
                data.Order = Convert.ToInt32(result["Order"].ToString());
                data.url = result["url"].ToString();
                data.DateS = result["DateS"].ToString();
                //string filename = result["hdnfile"].ToString();
                data.ImageUrl=result["hdnfile"].ToString();

                //NM.insertUpdate(data);
                return JavaScript("document.getElementById('status').innerHTML = 'Success';setTimeout(function(){$('#AddNew').data('kendoWindow').close();},1000)");
            }
            catch (Exception e)
            {
                return Content("Server Error!"); ;
            }
        }


        #endregion
    }
    public class ItemsObj
    {
        public int? Id;
        public string ImageUrl;
        public string DateS;
        public string Description;
        public Boolean isArabic;
        public string url;
        public int Order;
    }
    public class items
    {
        public int value { get; set; }
    }
}
