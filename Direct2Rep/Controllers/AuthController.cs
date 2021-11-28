using Direct2Rep.Helping_Classes;
using Direct2Rep.BL;
using Direct2Rep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading;

namespace Direct2Rep.Controllers
{
    public class AuthController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();




        public User validateUser()
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                  .Select(c => c.Value).SingleOrDefault();

            User loggedInUser = new AdminBL().getUserById(Convert.ToInt32(userId));

            return loggedInUser;
        }

        public ActionResult Login(string err = "", string passSet = "", string loginSuccessMsg = "")
        {

            if (sessiondto.getName() == null)
            {
                ViewBag.err = err;

                return View();
            }
            ViewBag.ConfirmPasswordUpdate = passSet;
            ViewBag.err = err;
            ViewBag.ConfirmloginSuccessMsg = loginSuccessMsg;

            return View();
        }

        [HttpPost]
        public ActionResult PostLogin(String email, String password)
        {
            User User = new AdminBL().getUserList().Where(x => x.Email == email && x.Password == password).FirstOrDefault();

            bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (isCapthcaValid)
            {
                if (User == null)
                {
                    Company company = new AdminBL().getCompanyList().Where(x => x.Email == email && x.Password == password).FirstOrDefault();

                    if (company == null)
                    {
                        return RedirectToAction("Login", new { err = "Invalid email or password. Try again!" });
                    }
                    else
                    {
                        var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name,company.Name),
                        new Claim(ClaimTypes.Email,company.Email),
                        new Claim("Id",company.Id.ToString()),
                        new Claim("Logo", company.Logo),
                        new Claim("Role", "10")

                   }, "ApplicationCookie");

                        var claimsPrincipal = new ClaimsPrincipal(identity);

                        // Set current principal
                        Thread.CurrentPrincipal = claimsPrincipal;
                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.SignIn(identity);

                        return RedirectToAction("Index", "Admin");
                    }
                }
                else if (User.Email == email && User.Password == password)
                {
                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name,User.Name),
                        new Claim(ClaimTypes.Email,User.Email),
                        new Claim("Id",User.Id.ToString()),
                        new Claim("Role", User.Role.ToString()),
                   }, "ApplicationCookie");

                    var claimsPrincipal = new ClaimsPrincipal(identity);

                    // Set current principal
                    Thread.CurrentPrincipal = claimsPrincipal;
                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", new { err = "Invalid email or password. Try again!" });
                }
            }
            else
            {
                return RedirectToAction("Login", "Auth", new { err = "Please ensure Captcha authenticity!!" });
            }
        }




        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login");
        }









        // GET: Auth
        //public ActionResult Login(string err = "")
        //{
        //    if (sessiondto.getName() == null)
        //    {
        //        ViewBag.err = err;

        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Admin");
        //    }
        //}

        public ActionResult Signup()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Login(String email, String password)
        //{
        //    User User = new AdminBL().getUserList().Where(x => x.Email == email && x.Password == password).FirstOrDefault();

        //    bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);
        //    if (isCapthcaValid)
        //    {
        //        if (User == null)
        //        {
        //            Company company = new AdminBL().getCompanyList().Where(x => x.Email == email && x.Password == password).FirstOrDefault();

        //            if (company == null)
        //            {
        //                return RedirectToAction("Login", new { err = "Invalid email or password. Try again!" });
        //            }
        //            else
        //            {
        //                SessionDTO session = new SessionDTO();
        //                session.Name = company.Name;
        //                session.Id = company.Id;
        //                session.Role = 10;
        //                session.Logo = company.Logo;
        //                Session["Session"] = session;

        //                SessionDTO sdto = (SessionDTO)Session["Sesssion"];

        //                return RedirectToAction("Index", "Admin");
        //            }
        //        }
        //        else if (User.Email == email && User.Password == password)
        //        {
        //            SessionDTO session = new SessionDTO();
        //            session.Name = User.Name;
        //            session.Id = User.Id;
        //            session.Role = Convert.ToInt32(User.Role);
        //            session.Logo = null;
        //            Session["Session"] = session;

        //            SessionDTO sdto = (SessionDTO)Session["Sesssion"];

        //            return RedirectToAction("Index", "Admin");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", new { err = "Invalid email or password. Try again!" });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Auth", new { err = "Please ensure Captcha authenticity!!" });
        //    }
        //}

        [HttpPost]
        public ActionResult PostSignup(User _user)
        {
            _user.Creadted_At = DateTime.Now;
            bool temp = new AdminBL().AddUser(_user);

            if (temp == false)
                return RedirectToAction("Signup", "Auth", new { err = "Please fill all fields carefully!" });

            return RedirectToAction("Login", "Auth");
        }

        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    Session.Abandon(); // it will clear the session at the end of request
        //    return RedirectToAction("Login", "Auth");
        //}

        public ActionResult EmailVerification(string err = "")
        {
            if (sessiondto.getName() == null)
            {
                ViewBag.message = err;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult PostEmailVerification(string Email)
        {
            User User = new AdminBL().getUserList().FirstOrDefault(x => x.Email == Email);
            Company company = new AdminBL().getCompanyList().Where(y => y.Email == Email).FirstOrDefault();
            if (User == null)
            {
                if(company != null)
                {
                    sendMail(Email);
                    return RedirectToAction("EmailVerification", "Auth", new { err = "We've sent a link to change your password to your Email address." });
                }
                return RedirectToAction("EmailVerification", "Auth", new { err = "The email address you entered was not found. Please call (213)291-7888 for assistance." });
            }
            else
            {
                sendMail(Email);
                return RedirectToAction("EmailVerification", "Auth", new { err = "We've sent a link to change your password to your Email address." });
            }
        }

        public bool sendMail(string email)
        {
            MailMessage msg = new MailMessage();
            string img = Url.Content("http://direct2rep.com/" + "Content/Web/assets/images/_smarty/D2RLogo200.png");
            string text = "<link href='https://fonts.googleapis.com/css?family=Bree+Serif' rel='stylesheet'><style>  * {";
            text += "  font-family: 'Bree Serif', serif; }";
            text += " .list-group-item {       border: none;  }    .hor {      border-bottom: 5px solid black;   }";
            text += " .line {       margin-bottom: 20px; }";

            msg.From = new MailAddress("no-reply@direct2rep.com");
            msg.To.Add(email);
            msg.Subject = "Password Reset";
            msg.IsBodyHtml = true;
            //string temp = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'></div> </nav><center><div><img src = 'logo'><h1 class='text-center'>Password Reset!</h1><p class='text-center'> Simply Click the button showing below to reset your password: </p><br><button style = 'background-color: rgb(0,174,239);'><a href='LINKFORFORGOTPASSWORD' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a></button></div></center>";
            string temp = "<html>" +
                    "<head></head>" +
                    "<body>" +
                    "<center>" +
                    "<img src="+ img +" style='width:250px; height:46px;'>" +
                    "<p class='text-center' style='color:#000000'> " +
                          "You have received this message because someone made a request to reset your password to our service. <br>" +
                          "If you did not make this request, you can safely ignore this message. " +
                    " </p>" +
                    "<p> " +
                    "If you made the password reset request, please click the link below to enter a new password:" +
                    "</p>" +
                    "<button style='background-color: #66ce31; padding:12px 16px; border:1px solid #66ce31; border-radius:3px;'>" +
                            "<a href='LINKFORFORGOTPASSWORD' style='text-decoration:none; font-size:15px; color:#000000;'> Reset Password </a>" +
                    "</button>" +
                    "</div>" + "</center>";
            temp += "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
            //string link = "http://localhost:63900/Auth/Reset?email=" + StringCipher.Base64Encode(email); 
            string link = "http://direct2rep.com/Auth/Reset?email=" + StringCipher.Base64Encode(email);
            //string link = "http://nodlayslahore-001-site3.btempurl.com/Auth/Reset?email=" + StringCipher.Base64Encode(email);
            link = link.Replace("+", "%20");
            temp = temp.Replace("LINKFORFORGOTPASSWORD", link);
            msg.Body = temp;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("no-reply@direct2rep.com", "nodlays@123");
                //client.Host = "mail.nodlays.com";
                client.Host = "mail.direct2rep.com";
                client.Port = 8889;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
            //using (SmtpClient smt = new SmtpClient())
            //{
            //    smt.Host = "smtp.gmail.com";
            //    System.Net.NetworkCredential ntwd = new NetworkCredential();
            //    ntwd.UserName = "madeyes1122@gmail.com"; //Your Email ID
            //    ntwd.Password = "qwerty_1L"; // Your Password
            //    smt.UseDefaultCredentials = false;
            //    smt.Credentials = ntwd;
            //    smt.Port = 587;
            //    smt.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smt.EnableSsl = false;
            //    smt.Send(msg);
            //}
            return true;
        }

        public ActionResult Reset(string email, string err = "")
        {
            email = StringCipher.Base64Decode(email);

            ViewBag.Email = email;
            ViewBag.message = err;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            User User = new AdminBL().getUserList().Where(x => x.Email == Email).FirstOrDefault();
            Company _Company = new AdminBL().getCompanyList().Where(x => x.Email == Email).FirstOrDefault();
            if (User == null)
            {
                if (_Company != null)
                {
                    if (Password == ConfirmPassword)
                    {
                        Company c = new Company
                        {
                            Id = _Company.Id,
                            Name = _Company.Name,
                            Email = _Company.Email,
                            Password = Password,
                            IsActive = _Company.IsActive,
                            Logo = _Company.Logo,
                            Phone = _Company.Phone,
                            Website = _Company.Website,
                            Creadted_At = _Company.Creadted_At

                        };
                        new AdminBL().UpdateCompany(c);
                        return RedirectToAction("Login", "Auth", new { err = "Password has been updated successfully." });
                    }
                }

            }
            if (Password == ConfirmPassword)
            {
                User.Password = Password;
                new AdminBL().UpdateUser(User);
                return RedirectToAction("Login", "Auth", new { err = "Password has been updated successfully." });
            }
            else
            {
                return RedirectToAction("Reset", "Auth", new { email = StringCipher.Base64Encode(Email), err = "Password and Confirm Password doesn't match" });

            }
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

        public ActionResult ChangePassword(int Id = -1, string msg = "", string err = "")
        {
            if (sessiondto.getId() == 0)
            {
                return RedirectToAction("Login", "Auth");
            }
            User user = new AdminBL().getUserById(Id);
            Company company = new AdminBL().getCompanyById(Id);
            ViewBag.Message = msg;
            ViewBag.Error = err;
            return View(user);
        }

        public ActionResult PostChangePassword(User _user, Company _company, string Password, string ConfirmPassword)
        {
            if (sessiondto.getId() == 0)
            {
                return RedirectToAction("Login", "Auth");
            }
            Direct2RepEntities db = new Direct2RepEntities();
            User User = new AdminBL().getUserList().Where(x => x.Id == sessiondto.getId()).FirstOrDefault();
            Company company = new AdminBL().getCompanyList().Where(x => x.Id == sessiondto.getId()).FirstOrDefault();
            if (sessiondto.getEmail() == User.Email)
            {
                if (Password == ConfirmPassword)
                {
                    User.Password = Password;
                    new AdminBL().UpdateUser(User);
                    return RedirectToAction("ChangePassword", "Auth", new { err = "Password has been updated successfully." });
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Auth", new { err = "Password and Confirm Password doesn't match" });
                }
                
            }
            //else if(sessiondto.getEmail() == company.Email)
            //{
            //    if (Password == ConfirmPassword)
            //    {
            //        company.Password = Password;
            //        new AdminBL().UpdateCompany(company);
            //    }
            //    return RedirectToAction("Login", "Auth", new { err = "Password has been updated successfully." });
            //}
            else
            {
                return RedirectToAction("ChangePassword", "Auth", new { err = "Password and Confirm Password doesn't match" });
            }
        }
    }
}
