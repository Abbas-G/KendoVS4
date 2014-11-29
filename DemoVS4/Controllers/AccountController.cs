using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoVS4.Models;
using System.Web.Security;

namespace DemoVS4.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult LogIn()
        {
            Session["Notification"] = "";
            Response.AppendHeader("X-LOGON", "true");
            return View();
        }

        public ActionResult HtmlAjax()
        {
            return View();
        }
        public ActionResult JSAjax()
        {
            return View();
        }

        public ActionResult logOn()
        {
            DemoVS4.Models.LogOnModel LM = new Models.LogOnModel();
            return PartialView("LogOn", LM);
        }

        [HttpPost]
        public ActionResult logOn(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                if (model.UserName=="abs" && model.Password=="abs")
                {

                    FormsAuthentication.Initialize();
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
                    Response.Cookies.Add(cookie);
                    if ((!String.IsNullOrEmpty(returnUrl)) && returnUrl.Length > 1)
                        return Redirect(returnUrl);
                    else
                    {
                        //return RedirectToAction("Grid", "Popup");

                        JavaScriptResult js = new JavaScriptResult();
                        js.Script = "gotoGrid();";
                        return js;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect user name or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("LogOn", model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn", "Account");

        }
    }
}
