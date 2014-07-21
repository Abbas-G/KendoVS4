using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoVS4.Core.Manager
{
    public class ProductManager
    {
        DemoVS4.Core.DAL.dbTestDataContext ctx = new DAL.dbTestDataContext();
        public int? insertUpdate(DridDataObj obj) { 
        
            int? iReturn = 0;
            ctx.spUpdateInsertProduct(obj.ProductID, obj.ProductName, obj.UnitPrice, obj.UnitsInStock, obj.Discontinued, obj.Category, ref iReturn);
            if (iReturn >= 0)
                return iReturn;
            else
                return null;
        }

        public string getUniqueCodeById(int id) {
           return ctx.Products.Where(x => x.ProductID == id).FirstOrDefault().UniqueCode;
        }

        public bool DeleteRecordById(int? id)
        {
            DemoVS4.Core.DAL.Product l = ctx.Products.Where(x => x.ProductID == id).FirstOrDefault();
            if (l != null)
            {
                ctx.Products.DeleteOnSubmit(l);
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

    }
   
    public class DridDataObj
    {
        public int? ProductID;
        public string ProductName;
        public string UniqueCode;
        public int UnitPrice;
        public int UnitsInStock;
        public Boolean Discontinued;
        public string Category;
        public DateTime CreatedDate;
        public int Duration;
    }
}
