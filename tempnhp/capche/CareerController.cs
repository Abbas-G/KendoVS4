using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TiaSolutions.Core.DAL;
using TiaSolutions.Core.Manager;
using System.Configuration;

namespace TiaSolutions.Web.Controllers
{
    public class CareerController : Controller
    {
        //
        // GET: /Career/
        private Random random = new Random();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Apply(string position)
        {
            ViewData["postion"] = position;
            Session["CaptchaImageText"] = GenerateRandomCode();
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = ".Net Developer", Value = "DotNet" });
            result.Add(new SelectListItem() { Text = "iPhone (ios)", Value = "Ios" });
            result.Add(new SelectListItem() { Text = "Web Designer", Value = "WebDesigner" });
            ViewData["postionList"] = result; 
            return View();
        }

        [HttpPost]
        public ActionResult Apply(HttpPostedFileBase Resume, FormCollection result)
        {
            
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = ".Net Developer", Value = "DotNet" });
            list.Add(new SelectListItem() { Text = "iPhone (ios)", Value = "Ios" });
            list.Add(new SelectListItem() { Text = "Web Designer", Value = "WebDesigner" });
            ViewData["postionList"] = list; 

            string FN= result["FirstName"].ToString();
            string LN = result["LastName"].ToString();
            string Age = result["Age"].ToString();
            string Gender = result["Gender"].ToString();
            string Email = result["Email"].ToString();
            string tel = result["tel"].ToString();
            string Edu = result["Educations"].ToString();
            string Notice = result["NoticePeriod"].ToString();
            string Designation = result["Designation"].ToString();
            string yr = result["year"].ToString();
            string mnth = result["Month"].ToString();
            string CSalary = result["CurrentSalary"].ToString();
            string ESalary = result["ExpectedSalary"].ToString();
            string City=result["City"].ToString();
            bool isValid = validateForm(FN, LN, Age, Gender, Email, tel, Edu, Notice, Designation, yr, mnth, CSalary, ESalary, City);

            if (isValid==true && result["Resume"] != "" && result["txtCodeNumber"] == this.Session["CaptchaImageText"].ToString())
            {
                // Display an informational message.
                string position = result["hdnPosition"].ToString();

                CareerManager cm= new CareerManager();
                Application data = new Application();
                data.FirstName = FN;
                data.LastName = LN;
                data.Age = Convert.ToInt32(Age);
                data.Gender = Gender;
                data.Email = Email;
                data.PhoneNo = tel;
                data.Education = Edu;
                data.NoticePeriodInWeek =Convert.ToInt32(Notice);
                data.Designation = Designation;
                ViewData["postion"] = Designation;
                data.TotalExperienceInYear = Convert.ToInt32(yr);
                data.TotalExperienceInMonth = Convert.ToInt32(mnth);
                data.City=City;
                data.CurrentSalaryInYear = Convert.ToDouble(CSalary);
                data.ExpectedSalaryInYear = Convert.ToDouble(ESalary);
                data.ResumeName = FN + "_" + Designation + "_Exp" + yr + "Yr" + mnth + "Mnth_" + Path.GetFileName(Resume.FileName);
                data.IpAddress = Request.UserHostAddress;
                data.Browser = Request.Browser.Browser;
                data.DateTime = System.DateTime.Now;
                data.IsArchive = false;
                data.IsICP = false;
                data.IsTP = false;
                data.Status = "Received";
                data.Feedback_BC = null;
                data.Feedback_SM = null;
                data.InterviewDate = null;
                data.Rating = 0;

                if (cm.InsertApplication(data))
                {
                ViewData["MessageLabel"] = "";
                string p = "";
                string fullfilename="";
                    try
                    {
                        try
                        {
                            string fileName = Path.GetFileName(Resume.FileName);
                            fullfilename = FN + "_" + Designation + "_Exp" + yr + "Yr" + mnth + "Mnth_"  +fileName;
                            var path = Path.Combine(Server.MapPath("~/Resumes/" + Designation), fullfilename);
                            p = path;
                            Resume.SaveAs(path);
                            //ViewData["MessageLabel"] = ViewData["MessageLabel"] + "<span>uploaded successfully at path : "+path+" </span><br/>";
                        }
                        catch (Exception e)
                        {
                            ViewData["MessageLabel"] = ViewData["MessageLabel"] + "<span>fail to upload file at path :" + p + "</span><br/>";
                        }
                        
                        string subject = "A New Resume is uploaded for the position of " + Designation + " Experience " + yr + " year & " + mnth + "Month";
                        string messageBody = "<div style='font:11px/1.35em Verdana, Arial, Helvetica, sans-serif;'><p>";
                        messageBody += "<b>Name    :&nbsp;</b>" + FN+ " "+LN+"<br>";
                        messageBody += "<b>Phone   :&nbsp;</b>" + tel + "<br>";
                        messageBody += "<b>Email    :&nbsp;</b>" + Email + "<br>";
                        messageBody += "<b>Designation    :&nbsp;</b>" + Designation + "<br>";
                        messageBody += "<b>Experience    :&nbsp;</b>" + yr + " year & " + mnth + "Month<br>";
                        messageBody += "<b>Gender :&nbsp;</b>" + Gender + "<br>";
                        messageBody += "<b>City :&nbsp;</b>" + City + "<br>";
                        string hostName = ConfigurationSettings.AppSettings["host"].ToString();
                        string serverPath = hostName + "Resumes/" + Designation + "/" + fullfilename;
                        messageBody += "<b>Resume Link:&nbsp;</b>" + "<a href='" + serverPath + "' >Download</a>";
                        messageBody += "<br>";
                        bool mail = TiaSolutions.Web.Models.Helper.SendEMail(subject, messageBody);
                        ViewData["MessageLabel"] = ViewData["MessageLabel"] + "<span><label style='color:white;background:#8ec724'>&nbsp;&nbsp;&nbsp;Form Submitted Successfully.&nbsp;&nbsp;&nbsp;</label></span>";
                    }
                    catch (Exception ex)
                    { }
                    //return RedirectToAction("Career", "Home");
                }
                return View("Apply", new { position = Designation });
            }
            else
            {
                // Display an error message.

                ViewData["MessageLabel"] = "<span><label style='color:red;font-size:large'>Error! please try again.</label></span>";

                // Clear the input and create a new random code.
               // ViewData["txtCodeNumber"] = "";
                this.Session["CaptchaImageText"] = GenerateRandomCode();
            }
            return View();
        }

        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
        }
        public ActionResult Refresh()
        {
            this.Session["CaptchaImageText"] = GenerateRandomCode();
            return PartialView("Refresh");
        }

        protected bool validateForm(string FN, string LN, string Age, string Gender, string Email, string tel, string Edu, string Notice, string Designation, string yr, string mnth, string CSalary, string ESalary, string City)
        {

            if (FN != "" && LN != "" && Age != "" && Gender != "" && Email != "" && tel != "" && Edu != "" && Notice != "" && Designation != "" && yr != "" && mnth != "" && CSalary != "" && ESalary != "" && City != "")
            {
                try {int a = Convert.ToInt32(Age);}
                catch (Exception ex)
                { return false; }

                try { int a = Convert.ToInt32(Notice); }
                catch (Exception ex)
                { return false; }

                try { int a = Convert.ToInt32(yr); }
                catch (Exception ex)
                { return false; }

                try { int a = Convert.ToInt32(mnth); }
                catch (Exception ex)
                { return false; }

                try { Double a = Convert.ToDouble(CSalary); }
                catch (Exception ex)
                { return false; }

                try { Double a = Convert.ToDouble(ESalary); }
                catch (Exception ex)
                { return false; }
                return true;
            }
            else
                return true;
            
        }
    }
}
