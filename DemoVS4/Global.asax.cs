using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoVS4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                //new { controller = "Grid", action = "inline", id = UrlParameter.Optional } // Parameter defaults
                //new { controller = "Grid", action = "PopUp", id = UrlParameter.Optional } 
                //new { controller = "Grid", action = "Mix", id = UrlParameter.Optional } 
                //new { controller = "Grid", action = "GridFromModel", id = UrlParameter.Optional } 
                
                  //new { controller = "ServerGrid", action = "Index", id = UrlParameter.Optional } 
                  //  new { controller = "ServerGridDetail", action = "Index", id = UrlParameter.Optional } 
                   // new { controller = "Grid", action = "InCell", id = UrlParameter.Optional } 

                   //new { controller = "GridMultiDropdown", action = "Index", id = UrlParameter.Optional } 
                   //new { controller = "GridMultiDropdown", action = "multiform", id = UrlParameter.Optional } 
                      // new { controller = "AjaxUploader", action = "Index", id = UrlParameter.Optional } 
                    //new { controller = "TVP", action = "xml", id = UrlParameter.Optional } 
                     // new { controller = "ServerInline", action = "Index", id = UrlParameter.Optional } 
                    // new { controller = "Link", action = "Index", id = UrlParameter.Optional } 
                   //  new { controller = "MultiModel", action = "tuple", id = UrlParameter.Optional } 
                      new { controller = "PartialViewChildAction", action = "Index", id = UrlParameter.Optional } 
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}