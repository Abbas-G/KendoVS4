using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCrack.Api.Models;
using System.Web.Script.Serialization;
using iCrack.Models;
using System.Drawing;
using System.IO;

namespace iCrack.Api.Controllers
{
    public class BaseController : Controller
    {
        internal ApiResponse CreateResponse(string Message, int Status, object Result)
        {
            ApiResponse data = new ApiResponse();
            data.Message = Message;
            data.Status = Status;
            data.Result = Result;
            return data;
        }

        internal ApiResponse CreateResponse(ApiResponse data)
        {
            return data;
        }

        internal T Deserialize<T>(string json)
        {
            JavaScriptSerializer jSerializer = new JavaScriptSerializer();
            return new JavaScriptSerializer().Deserialize<T>(json);
        }

        internal bool IsValidJsonSring(string json, out ApiResponse response)
        {
            ApiResponse NewResponse = new ApiResponse();
            if (string.IsNullOrEmpty(json))
            {
                NewResponse.Message = "Bad Request Null Data Passed";
                NewResponse.Status = (int)Status.Failure;
                NewResponse.Result = string.Empty;
                response = NewResponse;
                return false;
            }
            response = NewResponse;
            return true;
        }

        internal bool IsUserLogin(string LoginToken,int userid, iCrack.Core.DAL.dbiCrackDataContext ctx, out ApiResponse response)
        {
            ApiResponse NewResponse = new ApiResponse();
            iCrack.Core.DAL.DeviceTbl registration = ctx.DeviceTbls.Where(x => x.LoginToken == LoginToken && x.UserId == userid && x.IsLogin == true).FirstOrDefault();
            if (registration == null)
            {
                NewResponse.Message = "User Not Login";
                NewResponse.Status = (int)Status.Failure;
                NewResponse.Result = string.Empty;
                response = NewResponse;
                return false;
            }
            response = NewResponse;
            return true;
        }

        [NonAction]
        internal bool UploadImage(string Base64, string FileName, string Location, out string msg)
        {
            msg = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(Base64);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                    var path = Path.Combine(Location, FileName+".png");

                    image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
        }
    }
}
