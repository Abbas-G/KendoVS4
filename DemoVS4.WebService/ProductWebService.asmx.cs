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

using System.Web.Services.Protocols;

namespace DemoVS4.WebService
{
    /// <summary>
    /// Summary description for ProductWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProductWebService : System.Web.Services.WebService
    {

        JavaScriptSerializer jSerializer = new JavaScriptSerializer();
        public AuthHeader Authentication;

        [WebMethod(Description = "Read Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<DridDataObj> ReadProduct(string AnyThing)
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

        [WebMethod(Description = "Secure Read Product Detail")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [SoapHeader("Authentication", Required = true)]
        public List<DridDataObj> SecureReadProduct(string AnyThing)
        {
            if (Authentication.Password == "test")
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
            else
                return null;
        }
    }

    public class AuthHeader : SoapHeader
    {
        public string Password;
    }
}
