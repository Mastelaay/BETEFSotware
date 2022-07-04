using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace BETEFSotware.Managers
{
    public static class HelperManager
    {
        public static string CreateEmailBody(int statusID , string name)
        {
            string body;
            var readerToRender = StreamReader.Null;
            switch (statusID)
            {
                case 1:
                    readerToRender = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/Emails/UserEmail.cshtml"));
                    break;
               
            }
            using(var reader = readerToRender) { body = reader.ReadToEnd(); }
            body = body.Replace("{name}", name);
          
            return body;
        }

        public static string CreateEmailBody(int statusID, CheckoutModel cart, string orderNumber)
        {
            string body;
            var readerToRender = StreamReader.Null;
            var orderList = new List<string>();
            var sb = new StringBuilder();

            switch (statusID)
            {
                case 1:
                    readerToRender = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/Emails/UserEmail.cshtml"));
                    break;

            }

            using (var reader = readerToRender) { body = reader.ReadToEnd(); }
            foreach (var item in cart.Products)
            {
                var table = "<tr><td>" + item.ProductName + "</td><td>" + item.Price + "</td><td>" + item.Quantity + "</td></tr>";

                orderList.Add(table);

            }

            foreach (var item in orderList)
            {
                sb.Append(item);
                sb.AppendLine();
            }

            body = body.Replace("{orders}", sb.ToString());
            body = body.Replace("{userName}", cart.User.Name);
            body = body.Replace("{orderNumber}", orderNumber);
            body = body.Replace("{totalPrice}", cart.TotalPrice);

            return body;
        }

        public static void SendEmail (string subject , string body, string emailTo)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["VFrom"]);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(emailTo));

                    var smtp = new SmtpClient
                    {
                        Host = ConfigurationManager.AppSettings["VHost"],
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["VEnableSSL"]),
                        Port = 587

                    };
                    var networkCred = new System.Net.NetworkCredential { UserName = (ConfigurationManager.AppSettings["VUsername"]), Password = (ConfigurationManager.AppSettings["VPassword"]) };

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCred;
                   // smtp.Send(mailMessage); Google 
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}