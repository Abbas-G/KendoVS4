using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

using DemoVS4.Core;
using DemoVS4.Core.Manager;
using System.Data.Linq;

namespace DemoVS4.WebReference
{
    /// <summary>
    /// Summary description for MyWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyWebService : System.Web.Services.WebService
    {
        JavaScriptSerializer jSerializer = new JavaScriptSerializer();
        DemoVS4.Core.Manager.ProductManager PM = new ProductManager();


        [WebMethod(Description="return pure JSON")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ReadPureJson()
        {
            DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();
            List<DridDataObj> TempList = new List<DridDataObj>();

            foreach (DemoVS4.Core.DAL.Product m in List)
            {
                DridDataObj temp = new DridDataObj();
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice.Value, UnitsInStock = m.UnitsInStock.Value, Discontinued = m.Discontinued.Value, Category = m.Category, CreatedDate = m.CreatedDateTime.Value, Duration = 20 };
                TempList.Add(temp);
            }
            Context.Response.Write(jSerializer.Serialize(TempList));
        }

        [WebMethod(Description = "Read Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<DridDataObj> Read()
        {
            DemoVS4.Core.DAL.dbTestDataContext ctx = new Core.DAL.dbTestDataContext();
            List<DemoVS4.Core.DAL.Product> List = ctx.Products.ToList();
            List<DridDataObj> TempList = new List<DridDataObj>();

            foreach (DemoVS4.Core.DAL.Product m in List)
            {
                DridDataObj temp = new DridDataObj();
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice.Value, UnitsInStock = m.UnitsInStock.Value, Discontinued = m.Discontinued.Value, Category = m.Category, CreatedDate = m.CreatedDateTime.Value, Duration = 20 };
                TempList.Add(temp);
            }

            // return jSerializer.Serialize(TempList);
            return TempList;
        }

        [WebMethod(Description = "Update Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Update(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate, Duration = 20 };
            }
            temp.ProductID = PM.insertUpdate(temp);
            temp.UniqueCode = PM.getUniqueCodeById(temp.ProductID.Value);
            //return temp;
            Context.Response.Write(jSerializer.Serialize(temp));
        }

        [WebMethod(Description = "Delete Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Delete(string models)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            IList<DridDataObj> persons = new JavaScriptSerializer().Deserialize<IList<DridDataObj>>(models);
            DridDataObj temp = new DridDataObj();
            foreach (DridDataObj m in persons)
            {
                temp = new DridDataObj { ProductID = m.ProductID, ProductName = m.ProductName, UnitPrice = m.UnitPrice, UnitsInStock = m.UnitsInStock, Discontinued = m.Discontinued, Category = m.Category, CreatedDate = m.CreatedDate };
            }
            PM.DeleteRecordById(temp.ProductID);
            Context.Response.Write(jSerializer.Serialize(temp));
        }

        [WebMethod(Description = "Create Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Create(string models)
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
            Context.Response.Write(jSerializer.Serialize(temp));
        }
    }
}
