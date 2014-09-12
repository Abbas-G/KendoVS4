using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace TestLaneCove.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection result)
        {
            int flag = 0;
            int i = 1;

            foreach (var key in result.Keys)
            {
                if (key.ToString().StartsWith("RName"))
                {
                    string RName = !string.IsNullOrEmpty(result["RName" + i]) ? result["RName" + i].Trim() : "";
                    if (RName != "")
                    {
                        string RBrief = !string.IsNullOrEmpty(result["RBrief" + i]) ? result["RBrief" + i].Trim() : "";
                        if (RBrief != "")
                        {
                            flag = 1;
                        }
                    }

                    i++;
                }
            }

            if (flag == 1)
                ViewData["OnSuccess"] = "Records Inserted Succesfully";

            return View();
        }

        #region list
        public ActionResult List()
        {
            return View();
        }

        public JsonResult ListDataSelect()
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            var publicationTable = GetRoute();
            return Json(publicationTable);  
        }

        public JsonResult ListDataUpdate(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<RouteObj> persons = new JavaScriptSerializer().Deserialize<IList<RouteObj>>(models);
            RouteObj temp = new RouteObj();
            foreach (RouteObj m in persons)
            {
                temp = new RouteObj { RouteID = m.RouteID, RouteName = m.RouteName, RouteBrief = m.RouteBrief, CreatedDate = m.CreatedDate, ModifiedDate = m.ModifiedDate, IsActive = m.IsActive };
            }
            //return Json(publicationTable, JsonRequestBehavior.AllowGet); 
            return Json(temp);
        }

        public JsonResult ListDataDelete(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            return Json(new { value = "success" });
        }

        public JsonResult ListDataCreate(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<RouteObj> persons = new JavaScriptSerializer().Deserialize<IList<RouteObj>>(models);
            RouteObj temp = new RouteObj();
            foreach (RouteObj m in persons)
            {
                temp = new RouteObj { RouteID = 22, RouteName = m.RouteName, RouteBrief = m.RouteBrief, CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.Now, IsActive = m.IsActive };
            }
            return Json(temp);
        }

        public JsonResult CheckDuplication(string RouteName)
        {
            RouteObj items = GetRoute().Where(x => x.RouteName == RouteName).FirstOrDefault();
            if (items != null)
                return Json(new { value = "true" });
            else
                return Json(new { value = "false" });

        }

        [NonAction]
        public List<RouteObj> GetRoute()
        {
            var list = new List<RouteObj>{
            new RouteObj{ RouteID=1, RouteName="chai",RouteBrief="sa", CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=true},
            new RouteObj{ RouteID=2, RouteName="Khaman",RouteBrief="dsa", CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=false},
            new RouteObj{ RouteID=3, RouteName="Dhokla",RouteBrief="sd", CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=true},
            new RouteObj{ RouteID=4, RouteName="Vada pav",RouteBrief="ds", CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=false},
            new RouteObj{ RouteID=5, RouteName="Dhosa", RouteBrief="ds",CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=true},
            new RouteObj{ RouteID=6, RouteName="idli", RouteBrief="asd",CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=true,},
            new RouteObj{ RouteID=7, RouteName="Noodles",RouteBrief="da", CreatedDate=System.DateTime.Now, ModifiedDate=System.DateTime.Now,IsActive=false}
            };

            return list;
        }
        #endregion
    }

    public class RouteObj
    {
        public int RouteID;
        public string RouteName;
        public string RouteBrief;
        public DateTime? CreatedDate;
        public DateTime? ModifiedDate;
        public Boolean IsActive;
    }
}
