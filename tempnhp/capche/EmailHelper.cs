using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;
using System.Net;
using System.Configuration;

namespace TiaSolutions.Web.Models
{
    public class Helper
    {
        public static bool SendEMail(string subject, string messageBody)
        {
            try
            {
                SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["smtpServer"].ToString());
                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["UserID"].ToString(), ConfigurationSettings.AppSettings["Password"].ToString());
                client.Credentials = cred;
                client.Port = 587;
                client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["MailCC"].ToString()))
                {
                    MailAddress to = new MailAddress(ConfigurationSettings.AppSettings["MailTo"].ToString());
                    MailAddress from = new MailAddress("contact@iprogrammer.com.au", "TiaSolutions", System.Text.Encoding.UTF8);
                    MailAddress cc = new MailAddress(ConfigurationSettings.AppSettings["MailCC"].ToString());

                    MailAddress replyto = new MailAddress(ConfigurationSettings.AppSettings["ReplyTo"].ToString());
                    MailMessage message = new MailMessage(from, to);
                    message.CC.Add(cc);
                    message.ReplyTo = replyto;
                    message.Subject = subject;
                    message.Priority = MailPriority.High;
                    message.Body = messageBody;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }
                else
                {
                    MailAddress to = new MailAddress(ConfigurationSettings.AppSettings["MailTo"].ToString());
                    MailAddress from = new MailAddress("contact@iprogrammer.com.au", "TiaSolutions", System.Text.Encoding.UTF8);
                    //MailAddress cc = new MailAddress(ConfigurationSettings.AppSettings["MailCC"].ToString());

                    MailAddress replyto = new MailAddress(ConfigurationSettings.AppSettings["ReplyTo"].ToString());
                    MailMessage message = new MailMessage(from, to);
                    //message.CC.Add(cc);
                    message.ReplyTo = replyto;
                    message.Subject = subject;
                    message.Priority = MailPriority.High;
                    message.Body = messageBody;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendEMailToCandidate(string subject, string messageBody, string mailto)
        {
            try
            {
                messageBody += "<p style=color:#B00000>Notice of confidentiality: <br>";
                messageBody += "The information contained herein is intended only for the confidential use of the recipient. If the reader of this message is neither the intended recipient, nor the person responsible for delivering it to the intended recipient, you are hereby notified that you have received this communication in error, and that any review, dissemination, distribution, or copying of this communication is strictly prohibited. If you receive this in error, please notify the sender immediately by telephone, and destroy this e-mail message. </p>";

                SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["smtpServer"].ToString());
                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["UserID"].ToString(), ConfigurationSettings.AppSettings["Password"].ToString());
                client.Credentials = cred;
                client.Port = 587;
                client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailAddress to = new MailAddress(mailto);
                MailAddress from = new MailAddress("contact@iprogrammer.com.au", "TiaSolutions", System.Text.Encoding.UTF8);
                    //MailAddress cc = new MailAddress(ConfigurationSettings.AppSettings["MailCC"].ToString());

                MailAddress replyto = new MailAddress(ConfigurationSettings.AppSettings["ReplyTo"].ToString());
                MailMessage message = new MailMessage(from, to);
                    //message.CC.Add(cc);
                message.ReplyTo = replyto;
                message.Subject = subject;
                message.Priority = MailPriority.High;
                message.Body = messageBody;
                message.IsBodyHtml = true;
                client.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}