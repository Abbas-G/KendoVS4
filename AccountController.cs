using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using KIREIP.Web.Models;

namespace KIREIP.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult LogOn()
        {
            Session["Notification"] = "";
            Response.AppendHeader("X-LOGON", "true");
            return View();
        }
          [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            Session["Notification"] = "";
            if (ModelState.IsValid)
            {
                KIREIP.Core.Manager.UserManager CM = new KIREIP.Core.Manager.UserManager();
                KIREIP.Core.DAL.Login usr = CM.LoginUser(model.UserName, model.Password);
                if (usr != null)
                {
                    FormsAuthentication.Initialize();
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, usr.UserName.ToString(), DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
                    Response.Cookies.Add(cookie);
                    if ((!String.IsNullOrEmpty(returnUrl)) && returnUrl.Length > 1)
                        return Redirect(returnUrl);
                    else
                    {
                        return RedirectToAction("Index", "Message");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect user name or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

          public ActionResult LogOff()
          {
              FormsAuthentication.SignOut();
              return RedirectToAction("LogOn", "Account");

          }
    }
}
