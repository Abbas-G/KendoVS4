using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DemoVS4.Core.Manager
{
    public class Product2Manager
    {

        DemoVS4.Core.DAL.dbTestDataContext ctx = new DAL.dbTestDataContext();
        public int? insertXML(string xmlString)
        {
            int? iReturn = 0;
            XElement xmlTree = XElement.Parse(xmlString.ToString());
            ctx.InsertXMLDataIntoProduct2(xmlTree);
            if (iReturn >= 0)
                return iReturn;
            else
                return null;
        }
    }
}
