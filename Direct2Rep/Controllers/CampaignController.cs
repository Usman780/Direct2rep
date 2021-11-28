using Direct2Rep.BL;
using Direct2Rep.Helping_Classes;
using Direct2Rep.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace Direct2Rep.Controllers
{
    public class CampaignController : Controller
    {
        // GET: Campaign
        public ActionResult Index(string company = "", string cam = "", string msg="", string err="")
        {
            string url = "http://direct2rep.com/d2r/" + company + "/" + cam;
            //string url = "https://direct2rep.com/d2r/" + company + "/" + cam;
            //string url = "https://nodlayslahore-001-site3.btempurl.com/d2r/" + company + "/" + cam;
            //string url = "https://localhost:63900/d2r/" + company + "/" + cam;
            Campaign campaign = new AdminBL().getCampaignList().Where(x => x.Url == url).FirstOrDefault();

            if (campaign == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Company c = new AdminBL().getCompanyById(Convert.ToInt32(campaign.CompanyId));
            List<State> states = new AdminBL().getStateList();
            int statescount = states.Count();

            ViewBag.statescount = statescount;
            ViewBag.Message = msg;
            ViewBag.Error = err;
            ViewBag.co = company;
            ViewBag.ca = cam;
            ViewBag.company = c;
            ViewBag.campaign = campaign;
            ViewBag.abc = states;
            return View();
        }


        public static bool ValidateCaptcha(string response)
        {
            //secret that was generated in key value pair  
            string secret = "6LfI61QUAAAAAAFwcYFw9C1ZYSElBIgLMN1GmmKR";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);
        }

        [HttpPost]
        public ActionResult PostVisitor(FormCollection form)
        {

            bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (isCapthcaValid)
            {

                Company c = new AdminBL().getCompanyById(Convert.ToInt32(Request.Form["CompanyId"]));
                State s = new AdminBL().getStateList().Where(a => a.Id == Convert.ToInt32(Request.Form["StateCover"])).FirstOrDefault();
                if (s != null)
                {
                    int RepCount = new AdminBL().getRepStatePartList().Where(z => z.StateId == s.Id).Count();
                    if (RepCount == 0)
                    {
                        return RedirectToAction("Visitor", new { err = "The Sales Representative is not Available in this state Right Now. We Will Contact You As soon as Possible", companywebsite = StringCipher.Base64Encode(c.Website) });
                        //return RedirectToAction("Index", new { company = Request.Form["co"], cam = Request.Form["ca"], err = "The Sales Representative in Not Available in this State Right Now. We Will Contact You As Soon As Possible." });
                    }
                    else
                    {
                        Visitor vs = new Visitor()
                        {
                            Name = Request.Form["Name"],
                            BusinessName = Request.Form["BusinessName"],
                            Email = Request.Form["Email"],
                            Phone = Request.Form["Phone"],
                            Address = Request.Form["Address"],
                            City = Request.Form["City"],
                            ZipCode = Request.Form["ZipCode"],
                            BestMethod = Request.Form["BestMethod"],
                            BestTime = Request.Form["BestTime"],
                            Campaign_Id = Convert.ToInt32(Request.Form["Campaign_Id"]),
                            Message = Request.Form["Message"],
                            Creadted_At = DateTime.Now,
                            IsActive = 1
                        };



                        bool x = new AdminBL().AddVisitor(vs);

                        if (x)
                        {
                            if (Request.Form["StateCover"] != null)
                            {
                                VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover"]));
                            }
                            //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                            //{
                            //    if (Request.Form["StateCover" + i.ToString()] != null)
                            //    {
                            //if (Request.Form["PartofState"] == "All")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                            //}
                            //if (Request.Form["PartofState"] == "East")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                            //}
                            //if (Request.Form["PartofState"] == "West")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                            //}
                            //if (Request.Form["PartofState"] == "North")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                            //}
                            //if (Request.Form["PartofState"] == "South")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 4);
                            //}
                            //if (Request.Form["PartofState"] == "Metro")
                            //{
                            //    VisitorState(c.Id, vs.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 5);
                            //}
                            ////    }
                            ////}
                            return RedirectToAction("Visitor", new { msg = "Your sales representative will contact you.", companywebsite = StringCipher.Base64Encode(c.Website) });
                            // return RedirectToAction("VisitorState","Compaign");
                        }
                    }
                }
                
                return RedirectToAction("Index", new { company = Request.Form["co"], cam = Request.Form["ca"], err = "Please fill all fields carefully." });
                
            }
            else
            {
                return RedirectToAction("Index", new { company = Request.Form["co"], cam = Request.Form["ca"], msg = "Please ensure Captcha authenticity!!" });
            }

        }
        public void sendmail(string email, string content, string title)
        {
            try
            {
                var fromAddress = new MailAddress("no-reply@direct2rep.com", title);

                string fromPassword = "nodlays@123";
                string subject = title;
                string body = content;

                var smtp = new SmtpClient
                {

                    Host = "mail.direct2rep.com",
                    Port = 8889,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, new MailAddress(email))
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {

            }

        }
        public ActionResult Visitor(string msg= "", string companywebsite="", string err = "")
        {
            ViewBag.website = StringCipher.Base64Decode(companywebsite);
            ViewBag.message = msg;
            ViewBag.error = err;
            return View();
        }

        public bool VisitorState(int companyId, int VisitorId, int StateCoverId)
        {
            
            Company company = new AdminBL().getCompanyById(companyId);
            Visitor v = new AdminBL().getVisitorById(VisitorId);

            Visitor_States vs = new Visitor_States()
            {
                VisitorId = VisitorId,
                StateId = StateCoverId,
                Creadted_At = DateTime.Now,
                IsActive = 1
            };
            int count = new AdminBL().getVisitor_StatesList().Where(x => x.VisitorId == vs.VisitorId && x.StateId == vs.StateId).ToList().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                bool x = new AdminBL().AddVisitor_States(vs);

                List<Company_SaleRep> companyreps = new AdminBL().getCompany_SaleRepList().Where(y => y.CompanyId == companyId).ToList();
                List<SaleRepresentative> salesreps = new List<SaleRepresentative>();
                List<RepStatePart> repstateparts = new List<RepStatePart>();


                foreach (Company_SaleRep cs in companyreps)
                {
                    salesreps.Add(cs.SaleRepresentative);
                }


                foreach (SaleRepresentative s in salesreps)
                {
                    RepStatePart r = new AdminBL().getRepStatePartList().Where(z => z.SaleRepId == s.Id && z.StateId == StateCoverId).FirstOrDefault();

                    if (r != null)
                    {
                        repstateparts.Add(r);
                    }

                }
                if (repstateparts.Count > 0)
                {
                    foreach (RepStatePart rs in repstateparts)
                    {
                        MailMessage emsg = new MailMessage();

                        string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
                        text += "  font-family: 'Bree Serif', serif; }";
                        text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
                        text += " .line {       margin-bottom: 20px; }";

                        string img = Url.Content("http://direct2rep.com/" + company.Logo);
                        img = img.Replace("~", "");

                        emsg.From = new MailAddress("no-reply@direct2rep.com");
                        emsg.To.Add(rs.SaleRepresentative.EmailReceiveLeads);
                        emsg.Subject = "Customer Contact Request From "+ v.Campaign.Name;
                        emsg.IsBodyHtml = true;
                        string temp = "<html>"+
                            "<head></head>"+
                                "<body>"+
                                    "<center>"+
                                        "<div style='padding-left:150px; padding:-right:150px'>"+
                                            "<img src='" + img + "' height='75px' width='75px' alt='Logo' >" +
                                            "<br>" +
                                            "<h3> Hi " + rs.SaleRepresentative.Name + "</h3>" +
                                            "<p>" + 
                                                "This is a direct contact request from a customer in your territory " +
                                                "who opened and responded to the " + v.Campaign.Name + " campaign sent by " + company.Name + ".<br> " +
                                                "If the customer is not in your territory, please forward it to the " +
                                                "appropriate rep if known, or forward to "+ company.Name +"  at "+ company.Email +"." +
                                            "</p>" +
                                            "<strong> Compaign Name: </strong> " + v.Campaign.Name + " <br />" +
                                            "<strong> Contact Name: </strong> " + v.Name + " <br />" +
                                            "<strong> Business Name: </strong> " + v.BusinessName + " <br />" +
                                            "<strong>Email: </strong> " + v.Email + " <br />" +
                                            "<strong>Phone: </strong> " + v.Phone + " <br/>" +
                                            "<strong>Address: </strong> " + v.Address + "<br />" +
                                            "<strong>City: </strong> " + v.City + " <br />" +
                                            "<strong>State: </strong> " + rs.State.Name + " <br />" +
                                            "<strong>Zip Code: </strong> " + v.ZipCode + " <br />" +
                                            "<strong>Best Method: </strong> " + v.BestMethod + " <br />" +
                                            "<strong>Best Time: </strong> " + v.BestTime + "<br />" +
                                            "<strong>Message: </strong> " + v.Message + "<br />" +
                                            "</div>" +
                                            "<div>" +
                                            "<p>If you would like to adjust the frequency of these emails, please send your request to<br> support@direct2rep.com.</p>" +
                                            "</div>" +
                                            "</center>";
                        temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
                        emsg.Body = temp;
                        //sendmail(rs.SaleRepresentative.Email,temp,"Direct2Rep");
                        //sendmail(rs.SaleRepresentative.EmailReceiveLeads,temp,"Direct2Rep");
                        //using (SmtpClient smt = new SmtpClient())
                        //{
                        //    smt.Host = "smtp.gmail.com";
                        //    System.Net.NetworkCredential ntwd = new NetworkCredential();
                        //    ntwd.UserName = "madeyes1122@gmail.com"; //Your Email ID  
                        //    ntwd.Password = "qwerty_1L"; // Your Password  
                        //    smt.UseDefaultCredentials = false;
                        //    smt.Credentials = ntwd;
                        //    smt.Port = 587;
                        //    smt.EnableSsl = true;
                        //    smt.Send(msg);
                        //}
                        //run kro
                        using (SmtpClient client = new SmtpClient())
                        {
                            client.EnableSsl = false;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("no-reply@direct2rep.com", "nodlays@123");
                            client.Host = "mail.direct2rep.com";
                            client.Port = 8889;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.Send(emsg);
                        }
                    }
                }

                return x;
            }
        }
    }
}