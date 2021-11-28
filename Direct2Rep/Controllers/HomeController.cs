using Direct2Rep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Direct2Rep.Helping_Classes;
using Direct2Rep.BL;
using System.Net.Mail;
using System.Net;

namespace Direct2Rep.Controllers
{
    public class HomeController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();

        public ActionResult Index(string err = "")
        {
            //List<Company> c = new AdminBL().getCompanyList().ToList();

            //ViewBag.Companies = c;
            //ViewBag.CompaniesCount = c.Count();

            if (sessiondto.getName() == null)
            {
                ViewBag.err = err;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }      

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your about page.";

        //    return View();
        //}

        //public ActionResult Contact(string msg= "")
        //{
        //    ViewBag.Message = msg;

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult PostContact(string name, string email, string subject, string message)
        //{
        //    MailMessage msg = new MailMessage();

        //    msg.From = new MailAddress("support@casearea.com");
        //    msg.To.Add("hussnaindar17@gmail.com");
        //    msg.Subject = "Contact Us";
        //    msg.IsBodyHtml = true;
        //    msg.Body = "<strong>Name: </strong>" + name + "<br><strong>Email: </strong> " + email + "<br><strong>Subject: </strong> " + subject +
        //                "<br><strong>Message:</strong> " + message;

        //    using (SmtpClient client = new SmtpClient())
        //    {
        //        client.EnableSsl = false;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential("support@casearea.com", "delta@O27");
        //        client.Host = "webmail.casearea.com";
        //        client.Port = 25;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.Send(msg);
        //    }
        //    return RedirectToAction("Contact", "Home", new { msg = "Your message has been sent." });
        //}      
                
    }
}