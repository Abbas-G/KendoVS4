using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using JdSoft.Apple.Apns.Notifications;

namespace NHP.Web.Controllers
{
    public class CategoryController : Controller
    {
        int height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CategoryHeight"].ToString());
        int width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CategoryWidth"].ToString());
        NHP.Core.Manager.CategoriesManager cateManeger = new NHP.Core.Manager.CategoriesManager();
        NHP.Core.DAL.dbNHPDataContext ctx = new NHP.Core.DAL.dbNHPDataContext();

        public ActionResult Category()
        {
            ViewData["Category"] = "0";
            return View();
        }

        #region Category PDF
        public ActionResult AddCategoryDetails()
        {
            string v = Request.QueryString["Id"];
            int id=Convert.ToInt32(v);
            if (id != 0)
            {
                NHP.Core.DAL.CategorySub data = ctx.CategorySubs.Where(x => x.CategoryId == id && x.IsDelete == false).FirstOrDefault();
                if (cateManeger.isPDF(id))
                {
                    return PartialView("CategoryDetails", data);
                }
                else
                    return PartialView("CategoryVideoDetails", data);

            }
            else {
                return PartialView("Blank");
            }

        }

        [HttpPost]
        public ActionResult CategoryForm(int id,int pid,NHP.Core.DAL.CategorySub model,FormCollection result)
        {
            int submit = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenOnSubmit"]) ? result["HiddenOnSubmit"] : "0");
            if (submit == 2)
            {
                int flag = 0;
                int Count = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenCnt"]) ? result["HiddenCnt"] : "0");
                
                //if (result["CheckCurrentPDF"].ToString().Contains("true"))
               
                for (int i = 1; i <= Count; i++)
                {
                    string Thumbnailurl = !string.IsNullOrEmpty(result["TxtFilethumbnailUrl" + i]) ? result["TxtFilethumbnailUrl" + i].Trim() : "";
                    string fileurl = !string.IsNullOrEmpty(result["TxtFileUrl" + i]) ? result["TxtFileUrl" + i].Trim() : "";
                    string filename = !string.IsNullOrEmpty(result["TxtFileName" + i]) ? result["TxtFileName" + i] : "";
                    int fileid = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenFileId" + i]) ? result["HiddenFileId" + i] : "0");
                    int orderingid = Convert.ToInt32(!string.IsNullOrEmpty(result["reorderSelect" + i]) ? result["reorderSelect" + i] : null);
                    string version = !string.IsNullOrEmpty(result["TxtVersion" + i]) ? result["TxtVersion" + i] : "";
                    string check = cateManeger.getVersionbyfileid(fileid);
                    if (pid == 0)
                        cateManeger.updatePDF(fileid, id, fileurl, Thumbnailurl, filename, orderingid, version);
                    else
                        cateManeger.updatePDF(fileid, pid, fileurl, Thumbnailurl, filename, orderingid, version);

                    if (version.Equals(check)) { }
                    else {
                        if (result["CheckCurrentPDF"].ToString().Contains("true"))
                        {
                            SendNotification("A new version of " + cateManeger.getPDFNamebyfileid(fileid) + " is available, would you like to download it now?");
                            NHP.Core.Manager.DeviceToken coreDeviceToken = new NHP.Core.Manager.DeviceToken();
                            string chk= coreDeviceToken.SendAndroidMessage("A new version of " + cateManeger.getPDFNamebyfileid(fileid) + " is available, would you like to download it now?");
                        }
                    }
                    flag = 1;
                }
                ViewData["categoryId"] = id;
                ViewData["parentId"] = pid;
                if (flag == 1)
                ViewData["OnSuccessCurrentPDF"] = "Records Updated Succesfully";
                //return PartialView("CurrentCategoryPDF", data);
                return View("CategoryDetails", model);
            }
            else if (submit == 3)
            {
                int flag=0;
                int i = 1;
                int count= ctx.FileDetails.Where(x => x.CatId == id && x.ParId == pid && x.IsDelete == false).Count();
                //string isNotify;
                //if (result["CheckAddPDF"].ToString().Contains("true"))
                //    isNotify = "True";
                //else
                //    isNotify = "False";

                foreach (var key in result.Keys)
                {
                    if (key.ToString().StartsWith("file"))
                    {
                        string FileUrl = !string.IsNullOrEmpty(result["file" + i]) ? result["file" + i].Trim() : "";
                        if (FileUrl != "")
                        {
                            string Thumbnailurl = !string.IsNullOrEmpty(result["image" + i]) ? result["image" + i].Trim() : "";
                            if (Thumbnailurl != "")
                            {
                                string Version = !string.IsNullOrEmpty(result["TxtAddVersion" + i]) ? result["TxtAddVersion" + i] : "";
                                if (Version != "")
                                {
                                    count = count + 1;
                                    string filename = !string.IsNullOrEmpty(result["TxtFile" + i]) ? result["TxtFile" + i] : "";
                                    cateManeger.insertFilesWithCatId(id, pid, Thumbnailurl, FileUrl, filename, count, Version);
                                    string documentName;
                                    if (pid == 0)
                                        documentName = cateManeger.getCategoryNameByid(id);
                                    else
                                        documentName = cateManeger.getCategoryNameByid(pid);

                                    if (result["CheckAddPDF"].ToString().Contains("true"))
                                    {
                                        SendNotification("A new document is added: " + filename + " in " + documentName + " ");
                                        NHP.Core.Manager.DeviceToken coreDeviceToken = new NHP.Core.Manager.DeviceToken();
                                        string chk = coreDeviceToken.SendAndroidMessage("A new document is added: " + filename + " in " + documentName + " ");
                                    }
                                    
                                    flag = 1;
                                }
                            }
                        }

                        i++;
                    }
                }
                
                if(flag==1)
                    ViewData["OnSuccessAddPDF"] = "Records Inserted Succesfully";
                return View("CategoryDetails", model);
            }
            else
            {
                if (model.ThumbnailUrl == null)
                {
                    ViewData["ErrorCategoryThumb"] = "Please Select Thumbnail Url";
                    return View("CategoryDetails", model);
                }

                cateManeger.updateCategory(model.CategoryId, model.ParentId, model.ThumbnailUrl);
                ViewData["OnSuccess"] = "Records Updated Succesfully";

                return View("CategoryDetails", model);
            }
        }

        public ActionResult AddCurrentCategoryPDF()
        {
            string v = Request.QueryString["cid"];
            int id = Convert.ToInt32(v);
            string v1 = Request.QueryString["pid"];
            int pid = Convert.ToInt32(v1);

            List<NHP.Core.DAL.FileDetail> data = ctx.FileDetails.Where(x => x.CatId == id && x.ParId == pid && x.IsDelete == false).OrderBy(o => o.Reorder).ToList();
            //List<NHP.Core.DAL.FileDetail> data = (from c in ctx.FileDetails
            //                                      where c.CatId == id && c.ParId == pid && c.IsDelete == false
            //                                      orderby c.Reorder ascending
            //                                      select c).ToList();
            ViewData["categoryId"] = id;
            ViewData["parentId"] = pid;
            return PartialView("CurrentCategoryPDF", data);
        }

        public ActionResult DeleteCatPDF(int fid, int cid, int pid)
        {
            bool a = cateManeger.deletePDFByID(fid, cid, pid);
            List<NHP.Core.DAL.FileDetail> data = ctx.FileDetails.Where(x => x.CatId == cid && x.ParId == pid && x.IsDelete == false).OrderBy(o => o.Reorder).ToList();
            ViewData["categoryId"] = cid;
            ViewData["parentId"] = pid;
            return PartialView("CurrentCategoryPDF", data);

        }

        public ActionResult AddCategoryPDF()
        {
            string v = Request.QueryString["cid"];
            int id = Convert.ToInt32(v);
            string v1 = Request.QueryString["pid"];
            int pid = Convert.ToInt32(v1);
            ViewData["categoryId"] = id;
            ViewData["parentId"] = pid;
            return PartialView("CategoryPDF");

        }
        #endregion

        #region Category Video
        [HttpPost]
        public ActionResult CategoryVideoForm(int id, int pid, NHP.Core.DAL.CategorySub model, FormCollection result)
        {
            int submit = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenOnSubmit"]) ? result["HiddenOnSubmit"] : "0");
            if (submit == 2)
            {
                int flag = 0;
                int Count = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenCnt"]) ? result["HiddenCnt"] : "0");

                //if (result["CheckCurrentPDF"].ToString().Contains("true"))

                for (int i = 1; i <= Count; i++)
                {
                    string Thumbnailurl = !string.IsNullOrEmpty(result["TxtFilethumbnailUrl" + i]) ? result["TxtFilethumbnailUrl" + i].Trim() : "";
                    string fileurl = !string.IsNullOrEmpty(result["TxtFileUrl" + i]) ? result["TxtFileUrl" + i].Trim() : "empty.mp4"; //empty.mp4
                    string YouTubefileurl = !string.IsNullOrEmpty(result["TxtYouTubeFileUrl" + i]) ? result["TxtYouTubeFileUrl" + i] .Trim() : "empty"; //empty
                    string filename = !string.IsNullOrEmpty(result["TxtFileName" + i]) ? result["TxtFileName" + i] : "";
                    int fileid = Convert.ToInt32(!string.IsNullOrEmpty(result["HiddenFileId" + i]) ? result["HiddenFileId" + i] : "0");
                    int orderingid = Convert.ToInt32(!string.IsNullOrEmpty(result["reorderSelect" + i]) ? result["reorderSelect" + i] : null);
                    string version = !string.IsNullOrEmpty(result["TxtVersion" + i]) ? result["TxtVersion" + i] : "";
                    string check = cateManeger.getVersionbyfileid(fileid);
                    if (pid == 0)
                        cateManeger.updateVideo(fileid, id, fileurl, YouTubefileurl,Thumbnailurl, filename, orderingid, version);
                    else
                        cateManeger.updateVideo(fileid, pid, fileurl, YouTubefileurl,Thumbnailurl, filename, orderingid, version);

                    if (version.Equals(check)) { }
                    else
                    {
                        if (result["CheckCurrentPDF"].ToString().Contains("true"))
                        {
                            SendNotification("A new version of " + cateManeger.getPDFNamebyfileid(fileid) + " video is available, would you like to download it now?");
                            NHP.Core.Manager.DeviceToken coreDeviceToken = new NHP.Core.Manager.DeviceToken();
                            string chk = coreDeviceToken.SendAndroidMessage("A new version of " + cateManeger.getPDFNamebyfileid(fileid) + " video is available, would you like to download it now?");
                        }
                    }
                    flag = 1;
                }
                ViewData["categoryId"] = id;
                ViewData["parentId"] = pid;
                if (flag == 1)
                    ViewData["OnSuccessCurrentPDF"] = "Records Updated Succesfully";
                //return PartialView("CurrentCategoryPDF", data);
                return View("CategoryVideoDetails", model);
            }
            else if (submit == 3)
            {
                int flag = 0;
                int i = 1;
                int count = ctx.FileDetails.Where(x => x.CatId == id && x.ParId == pid && x.IsDelete == false).Count();
                //string isNotify;
                //if (result["CheckAddPDF"].ToString().Contains("true"))
                //    isNotify = "True";
                //else
                //    isNotify = "False";

                foreach (var key in result.Keys)
                {
                    if (key.ToString().StartsWith("file"))
                    {
                        string FileUrl = !string.IsNullOrEmpty(result["file" + i]) ? result["file" + i].Trim() : "empty.mp4";//empty.mp4
                        if (FileUrl != "")
                        {
                            string YouTubeurl = !string.IsNullOrEmpty(result["youtubefile" + i]) ? result["youtubefile" + i].Trim() : "empty";//empty
                            if (YouTubeurl != "")
                            {
                                string Thumbnailurl = !string.IsNullOrEmpty(result["image" + i]) ? result["image" + i].Trim() : "";
                                if (Thumbnailurl != "")
                                {
                                    string Version = !string.IsNullOrEmpty(result["TxtAddVersion" + i]) ? result["TxtAddVersion" + i] : "";
                                    if (Version != "")
                                    {
                                        count = count + 1;
                                        string filename = !string.IsNullOrEmpty(result["TxtFile" + i]) ? result["TxtFile" + i] : "";
                                        cateManeger.insertVideoFilesWithCatId(id, pid, YouTubeurl, Thumbnailurl, FileUrl, filename, count, Version);
                                        string documentName;
                                        if (pid == 0)
                                            documentName = cateManeger.getCategoryNameByid(id);
                                        else
                                            documentName = cateManeger.getCategoryNameByid(pid);

                                        if (result["CheckAddPDF"].ToString().Contains("true"))
                                        {
                                            SendNotification("A new video is added: " + filename + " in " + documentName + " ");
                                            NHP.Core.Manager.DeviceToken coreDeviceToken = new NHP.Core.Manager.DeviceToken();
                                            string chk = coreDeviceToken.SendAndroidMessage("A new video is added: " + filename + " in " + documentName + " ");
                                        }

                                        flag = 1;
                                    }
                                }
                            }
                        }

                        i++;
                    }
                }

                if (flag == 1)
                    ViewData["OnSuccessAddPDF"] = "Records Inserted Succesfully";
                return View("CategoryVideoDetails", model);
            }
            else
            {
                if (model.ThumbnailUrl == null)
                {
                    ViewData["ErrorCategoryThumb"] = "Please Select Thumbnail Url";
                    return View("CategoryVideoDetails", model);
                }

                cateManeger.updateCategory(model.CategoryId, model.ParentId, model.ThumbnailUrl);
                ViewData["OnSuccess"] = "Records Updated Succesfully";

                return View("CategoryVideoDetails", model);
            }
        }

        public ActionResult AddCurrentCategoryVideo()
        {
            string v = Request.QueryString["cid"];
            int id = Convert.ToInt32(v);
            string v1 = Request.QueryString["pid"];
            int pid = Convert.ToInt32(v1);

            List<NHP.Core.DAL.FileDetail> data = ctx.FileDetails.Where(x => x.CatId == id && x.ParId == pid && x.IsDelete == false).OrderBy(o => o.Reorder).ToList();
            //List<NHP.Core.DAL.FileDetail> data = (from c in ctx.FileDetails
            //                                      where c.CatId == id && c.ParId == pid && c.IsDelete == false
            //                                      orderby c.Reorder ascending
            //                                      select c).ToList();
            ViewData["categoryId"] = id;
            ViewData["parentId"] = pid;
            return PartialView("CurrentCategoryVideo", data);
        }

        public ActionResult AddCategoryVideo()
        {
            string v = Request.QueryString["cid"];
            int id = Convert.ToInt32(v);
            string v1 = Request.QueryString["pid"];
            int pid = Convert.ToInt32(v1);
            ViewData["categoryId"] = id;
            ViewData["parentId"] = pid;
            return PartialView("CategoryVideo");

        }

        public ActionResult DeleteCatVideo(int fid, int cid, int pid)
        {
            bool a = cateManeger.deletePDFByID(fid, cid, pid);
            List<NHP.Core.DAL.FileDetail> data = ctx.FileDetails.Where(x => x.CatId == cid && x.ParId == pid && x.IsDelete == false).OrderBy(o => o.Reorder).ToList();
            ViewData["categoryId"] = cid;
            ViewData["parentId"] = pid;
            return PartialView("CurrentCategoryVideo", data);

        }
        #endregion

        #region Phone Notification
        public void SendNotification(string message)
        {
            message = ""+message;

            NHP.Core.Manager.DeviceToken coreDeviceToken = new NHP.Core.Manager.DeviceToken();
            List<NHP.Core.DAL.IPhoneDeviceToken> phonedeviceTokens = coreDeviceToken.GetAllPhoneDeviceToken();
            if (phonedeviceTokens != null && phonedeviceTokens.Count() > 0)
            {
                foreach (NHP.Core.DAL.IPhoneDeviceToken _phoneDeviceToken in phonedeviceTokens)
                {
                    Notification(message, _phoneDeviceToken.DeviceToken.Trim());
                }
            }
        }

        
        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_BadDeviceToken :-> " + ex.Message.ToString());
        }

        static void service_Disconnected(object sender)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_Disconnected :-> ");
        }

        static void service_Connected(object sender)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_Connected 1 :-> ");
        }

        static void service_Connecting(object sender)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_Connecting 2 :-> ");
        }

        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_NotificationTooLong  :-> " + ex.StackTrace.ToString());
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_NotificationSuccess :-> ");
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_NotificationFailed :-> ");
        }

        static void service_Error(object sender, Exception ex)
        {
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();
            errorLog.AddErrorLog("service_Error :-> " + ex.Message.ToString());
        }

        public void Notification(string strnotificationmessage, string strDeviceToken)
        {
            string p12File = string.Empty;
            string p12FilePassword = string.Empty;
            NHP.Core.Manager.IphoneErrorLog errorLog = new NHP.Core.Manager.IphoneErrorLog();

            try
            {
                errorLog.AddErrorLog("Start");

                bool sandbox = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["sandbox"].ToString());
                string testDeviceToken = strDeviceToken.Trim();
                var physicalpath = Request.PhysicalApplicationPath;
                // p12File = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["p12File"].ToString().Trim());
                p12File = physicalpath.ToString() + System.Configuration.ConfigurationManager.AppSettings["p12File"].ToString().Trim();
                p12FilePassword = System.Configuration.ConfigurationManager.AppSettings["p12FilePassword"].ToString().Trim();
                int count = 1;
                int sleepBetweenNotifications = 15000;

                errorLog.AddErrorLog("Declare");

                string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

                NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

                errorLog.AddErrorLog("Notification");

                service.SendRetries = 5;
                service.ReconnectDelay = 5000; //5 seconds

                service.Error += new NotificationService.OnError(service_Error);
                service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

                errorLog.AddErrorLog("NotificationTooLong");

                service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
                service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
                service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
                service.Connecting += new NotificationService.OnConnecting(service_Connecting);
                service.Connected += new NotificationService.OnConnected(service_Connected);
                service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

                errorLog.AddErrorLog("SetDeviceToken");

                for (int i = 1; i <= count; i++)
                {

                    Notification alertNotification = new Notification(testDeviceToken.Trim());

                    alertNotification.Payload.Alert.Body = strnotificationmessage.Trim();
                    alertNotification.Payload.Sound = "default";
                    alertNotification.Payload.Badge = cateManeger.getCounterbyDeviceToken(testDeviceToken.Trim());
                    //alertNotification.Payload.Badge = i;


                    if (service.QueueNotification(alertNotification))
                    {
                        //lblnotificationlog.Text = lblnotificationlog.Text + " Notification Queued!";
                        errorLog.AddErrorLog("Notification Queued!");
                    }
                    else
                    {
                        //lblnotificationlog.Text = lblnotificationlog.Text + " Notification Failed to be Queued!";
                        errorLog.AddErrorLog("Notification Failed to be Queued!");
                    }
                    if (i < count)
                    {
                        errorLog.AddErrorLog("Sleep");
                        System.Threading.Thread.Sleep(sleepBetweenNotifications);
                    }
                }
                //lblnotificationlog.Text = lblnotificationlog.Text + ", Notification message sent successfully.";
                errorLog.AddErrorLog("Notification message sent successfully.(" + strnotificationmessage + ")");

                service.Close();
                errorLog.AddErrorLog("ServiceClose");

                service.Dispose();
                errorLog.AddErrorLog("End");

                cateManeger.addCounterbyDeviceToken(testDeviceToken.Trim(), cateManeger.getCounterbyDeviceToken(testDeviceToken.Trim()) + 1);
                
            }
            catch (Exception ex)
            {
                errorLog.AddErrorLog("Error : ->" + "p12File :-> " + p12File + "   p12FilePassword :->" + p12FilePassword + "   ErrorMessage" + ex.Message.ToString());
            }
        }

        #endregion

    }
}
