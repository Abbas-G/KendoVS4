using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiaSolutions.Core.DAL;

namespace TiaSolutions.Core.Manager
{
    public class AdminManager
    {
        TiaSolutions.Core.DAL.dbTiaSolutionsDataContext ctx = new TiaSolutions.Core.DAL.dbTiaSolutionsDataContext();
        public Login LoginUser(String Username, String Password)
        {
            Login user = ctx.Logins.Where(x => x.UserName == Username && x.Password == Password && x.IsDelete==false).FirstOrDefault();
            if (user != null)
                return user;
            else
                return null;
        }

        public List<TiaSolutions.Core.DAL.Application> getApplicantDetail()
        {
            List<TiaSolutions.Core.DAL.Application> l = ctx.Applications.Where(x => x.IsArchive == false && x.IsICP == false && x.IsTP == false).OrderByDescending(p => p.Id).ToList();
            if (l != null)
                return l;
            else
                return null;
        }
        public List<TiaSolutions.Core.DAL.Application> getICPDetail()
        {
            List<TiaSolutions.Core.DAL.Application> l = ctx.Applications.Where(x => x.IsArchive == false && x.IsICP == true && x.IsTP == false).OrderByDescending(p => p.Id).ToList();
            /*List<TiaSolutions.Core.DAL.Application> t = ctx.Applications.Where(x => x.IsArchive == false).ToList();
            foreach (var m in t)
            {
                //string a = m.InterviewDate.Value.Hour.ToString("00") + m.InterviewDate.Value.Minute.ToString("00");
                DateTime? a = m.InterviewDate;
                if (!string.IsNullOrEmpty(a.ToString()))
                {
                    string b=m.InterviewDate.Value.ToShortTimeString();
                }
            }*/
            if (l != null)
                return l;
            else
                return null;
        }
        public List<TiaSolutions.Core.DAL.Application> getTPDetail()
        {
            List<TiaSolutions.Core.DAL.Application> l = ctx.Applications.Where(x => x.IsArchive == false && x.IsICP == false && x.IsTP == true).OrderByDescending(p => p.DateTime).ToList();
            if (l != null)
                return l;
            else
                return null;
        }

        public List<TiaSolutions.Core.DAL.Application> getArchiveDetail()
        {
            List<TiaSolutions.Core.DAL.Application> l = ctx.Applications.Where(x => x.IsArchive == true && x.IsICP == false && x.IsTP == false).OrderByDescending(p => p.DateTime).ToList();
            if (l != null)
                return l;
            else
                return null;
        }

        public List<string> getUniqueGroubyData()
        {
            var a = (from c in ctx.Applications
                     where c.IsArchive == false
                     group c by c.Designation into d
                     select new
                     {
                         GroupByName = d.Key,
                     }); //groupby Food by category

            List<string> GroupByname = new List<string>();
            foreach (var m in a)
            {
                GroupByname.Add(m.GroupByName);
            }
            return GroupByname;
        }
        public bool DeleteRecordById(int id)
        {
            TiaSolutions.Core.DAL.Application l = ctx.Applications.Where(x => x.Id==id).FirstOrDefault();
            if (l != null)
            {
                ctx.Applications.DeleteOnSubmit(l);
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

        public bool MoveToArchiveById(int id)
        {
            TiaSolutions.Core.DAL.Application l = ctx.Applications.Where(x => x.Id == id).FirstOrDefault();
            if (l != null)
            {
                //l.DateTime = System.DateTime.Now;
                l.IsArchive = true;
                l.IsICP = false;
                l.IsTP = false;
                l.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

        public bool MoveToICPById(int id)
        {
            TiaSolutions.Core.DAL.Application l = ctx.Applications.Where(x => x.Id == id).FirstOrDefault();
            if (l != null)
            {
                //l.DateTime = System.DateTime.Now;
                l.IsArchive = false;
                l.IsICP = true;
                l.IsTP = false;
                l.Status = "Received";
                l.Feedback_SM = "";
                l.Feedback_BC = "";
                l.Rating = 0;
                l.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

        public bool MoveToTPById(int id)
        {
            TiaSolutions.Core.DAL.Application l = ctx.Applications.Where(x => x.Id == id).FirstOrDefault();
            if (l != null)
            {
                //l.DateTime = System.DateTime.Now;
                l.IsArchive = false;
                l.IsICP = false;
                l.IsTP = true;
                l.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

        public Boolean UpdateInfo(TiaSolutions.Core.DAL.Application data)
        {
            TiaSolutions.Core.DAL.Application k = ctx.Applications.Where(x => x.Id == data.Id && x.IsArchive == false).FirstOrDefault();
            if (k != null)
            {
                k.Rating = data.Rating;
                k.Status = data.Status;
                k.Feedback_SM = data.Feedback_SM;
                k.Feedback_BC = data.Feedback_BC;
                k.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }

        public Boolean UpdateInfoMail(TiaSolutions.Core.DAL.Application data)
        {
            TiaSolutions.Core.DAL.Application k = ctx.Applications.Where(x => x.Id == data.Id && x.IsArchive == false).FirstOrDefault();
            if (k != null)
            {
                k.InterviewDate = data.InterviewDate;
                k.Status = data.Status;
                k.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }
        public Boolean ModifyMailSchedule(TiaSolutions.Core.DAL.Application data)
        {
            TiaSolutions.Core.DAL.Application k = ctx.Applications.Where(x => x.Id == data.Id && x.IsArchive == false).FirstOrDefault();
            if (k != null)
            {
                k.InterviewDate = data.InterviewDate;
                k.DateTime = System.DateTime.Now;
                ctx.SubmitChanges();
                return true;
            }
            else
                return false;
        }
    }
}
