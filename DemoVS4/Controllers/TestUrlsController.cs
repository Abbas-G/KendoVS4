using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Routing;

namespace DemoVS4.Controllers
{
    public class TestUrlsController : Controller
    {
        //
        // GET: /TestUrls/

        #region persist url

        public ActionResult Index()
        {
            return RedirectToRoute("GotoTest");//use RedirectToRoute instaed of using RedirectToAction in you post action to persist URLs state.
            //means url want change http://stackoverflow.com/questions/26771008/url-changed-automatically
            //return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        #endregion

        #region RedirecrPerment
        public ActionResult News()
        {
            return new PermanentRedirectResult("http://stackoverflow.com/questions/1693548/correct-controller-code-for-a-301-redirect");
        }
        #endregion

    }

    public class PermanentRedirectResult : ActionResult
    {
        public string Url { get; set; }

        public PermanentRedirectResult(string url)
        {
            Url = url;
        }

        public PermanentRedirectResult(RequestContext context, string actionName, string controllerName)
        {
            UrlHelper urlHelper = new UrlHelper(context);
            string url = urlHelper.Action(actionName, controllerName);

            Url = url;
        }

        public PermanentRedirectResult(RequestContext context, string actionName, string controllerName, object values)
        {
            UrlHelper urlHelper = new UrlHelper(context);
            string url = urlHelper.Action(actionName, controllerName, values);

            Url = url;
        }

        public PermanentRedirectResult(RequestContext context, string actionName, string controllerName, RouteValueDictionary values)
        {
            UrlHelper urlHelper = new UrlHelper(context);
            string url = urlHelper.Action(actionName, controllerName, values);

            Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = Url;
            context.HttpContext.Response.End();
        }
    }
}
