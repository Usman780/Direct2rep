using Direct2Rep.BL;
using Direct2Rep.Helping_Classes;
using Direct2Rep.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Direct2Rep.Controllers
{
    public class CompanyController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();

        #region Sales Representatives
        public ActionResult ListSaleRep()
        {
            if (sessiondto.getRole() == 10)
            {
                List<Company_SaleRep> companysalereps = new AdminBL().getCompany_SaleRepList().Where(x => x.IsActive == 1 && x.CompanyId == sessiondto.getId()).ToList();
                ViewBag.companysalerep = companysalereps;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        //Hassan Codes
        //public ActionResult ListSaleRep(string err, string msg)
        //{
        //    if (sessiondto.getRole() != 10)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    ViewBag.Message = msg;
        //    ViewBag.Error = err;
        //    //List<SaleRepresentative> lists = new AdminBL().getSaleRepresentativeList().OrderBy(x => x.Creadted_At).ToList();
        //    List<Company_SaleRep> lists = new AdminBL().getCompany_SaleRepList().OrderBy(x => x.Creadted_At).ToList();

        //    //List<SaleRepresentative> co_rp_list = new List<SaleRepresentative>();
        //    List<Company_SaleRep> co_rp_list = new List<Company_SaleRep>();

        //    string str = "";
        //    foreach (Company_SaleRep x in lists)
        //    {
        //        if (x.SaleRepresentative.LogoUpload != null && x.SaleRepresentative.PictureUpload == null)
        //        {
        //            if (x.SaleRepresentative.LogoUpload.Contains("~"))
        //            {
        //                x.SaleRepresentative.LogoUpload = x.SaleRepresentative.LogoUpload.Replace('~', ' ');

        //                x.SaleRepresentative.LogoUpload.Trim();

        //                str = "<td><span>No Picture Found</span> / <a target='_blank' href=" + x.SaleRepresentative.LogoUpload + ">Logo</a> </td>";
        //            }
        //        }
        //        else if (x.SaleRepresentative.LogoUpload == null && x.SaleRepresentative.PictureUpload != null)
        //        {
        //            if (x.SaleRepresentative.PictureUpload.Contains("~"))
        //            {
        //                x.SaleRepresentative.PictureUpload = x.SaleRepresentative.PictureUpload.Replace('~', ' ');

        //                x.SaleRepresentative.PictureUpload.Trim();

        //                str = "<td> <a target='_blank' href=" + x.SaleRepresentative.PictureUpload + ">Picture</a> / <span>No Logo Found</span> </td>";
        //            }
        //        }
        //        else if (x.SaleRepresentative.LogoUpload != null && x.SaleRepresentative.PictureUpload != null)
        //        {
        //            if (x.SaleRepresentative.PictureUpload.Contains("~") || x.SaleRepresentative.LogoUpload.Contains("~"))
        //            {
        //                x.SaleRepresentative.PictureUpload = x.SaleRepresentative.PictureUpload.Replace('~', ' ');
        //                x.SaleRepresentative.LogoUpload = x.SaleRepresentative.LogoUpload.Replace('~', ' ');

        //                x.SaleRepresentative.PictureUpload.Trim();
        //                x.SaleRepresentative.LogoUpload.Trim();

        //                str = "<td> <a target='_blank' href=" + x.SaleRepresentative.PictureUpload + ">Picture</a> / <a target='_blank' href=" + x.SaleRepresentative.LogoUpload + ">Logo</a> </td>";
        //            }
        //        }
        //        else
        //        {
        //            str = "No Logo and No Image";
        //        }

        //        Company_SaleRep corpDTO = new Company_SaleRep()
        //        {
        //            Id = x.Id,
        //            //Name = x.SaleRepresentative.Name,
        //            //SalesFirmName = x.SaleRepresentative.SalesFirmName,
        //            //Email = x.SaleRepresentative.Email,
        //            //EmailReceiveLeads = x.SaleRepresentative.EmailReceiveLeads,
        //            //PictureUpload = x.SaleRepresentative.PictureUpload,
        //            //LogoUpload = x.SaleRepresentative.LogoUpload,
        //            CompanyId = x.CompanyId,
        //            SaleRepId = x.SaleRepId,
        //            IsActive = x.IsActive,
        //            Creadted_At = x.Creadted_At
        //        };

        //        co_rp_list.Add(corpDTO);
        //        ViewBag.salerep = co_rp_list;
        //    }
        //    return View();
        //}




        public ActionResult AddSaleRep(string err = "", string msg = "")
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<State> states = new AdminBL().getStateList();
            int statescount = states.Count();

            ViewBag.statescount = statescount;
            ViewBag.states = states;
            ViewBag.Message = msg;
            ViewBag.Error = err;
            ViewBag.sts = states;
            return View();
        }

        [HttpPost]
        public ActionResult PostSaleRep(FormCollection form)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            SaleRepresentative sr = new SaleRepresentative()
            {
                Name = Request.Form["Name"],
                SalesFirmName = Request.Form["SalesFirmName"],
                Email = Request.Form["Email"],
                EmailReceiveLeads = Request.Form["EmailReceiveLeads"],
                Creadted_At = DateTime.Now,
                IsActive = 1
            };

            sr.PictureUpload = null;
            sr.LogoUpload = null;
            var file = Request.Files[0];
            var file1 = Request.Files[1];
            if (file.ContentLength > 0)
            {
                Directory.CreateDirectory(Server.MapPath("~") + "Content\\SalesRepImages\\");
                string relativePath = "~/Content/SalesRepImages/" + Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath(relativePath));
                file.SaveAs(path);
                sr.PictureUpload = relativePath;
            }

            if (file1.ContentLength > 0)
            {
                Directory.CreateDirectory(Server.MapPath("~") + "Content\\SalesRepImages\\");
                string relativePath1 = "~/Content/SalesRepImages/" + Path.GetFileName(file1.FileName);
                string path1 = Path.Combine(Server.MapPath(relativePath1));
                file1.SaveAs(path1);
                sr.LogoUpload = relativePath1;
            }

            bool x = new AdminBL().AddSaleRepresentative(sr);
            if (x)
            {
                for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                {
                    if (Request.Form["StateCover"] != null)
                    {
                        repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover"]) + i);
                    }
                }
                //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                //{
                //    if (Request.Form["StateCover" + i.ToString()] != null)
                //    {
                //        if (Request.Form["PartofState" + i.ToString() + "_1"] == "All")
                //        {
                //            //for (int j = 0; j < 5; j++)
                //            //{
                //            //    repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + j);
                //            //}  
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                //        }
                //        if (Request.Form["PartofState" + i.ToString() + "_2"] == "East")
                //        {
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                //        }
                //        if (Request.Form["PartofState" + i.ToString() + "_3"] == "West")
                //        {
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                //        }
                //        if (Request.Form["PartofState" + i.ToString() + "_4"] == "North")
                //        {
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                //        }
                //        if (Request.Form["PartofState" + i.ToString() + "_5"] == "South")
                //        {
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 4);
                //        }
                //        if (Request.Form["PartofState" + i.ToString() + "_6"] == "Metro")
                //        {
                //            repStatePart(sr.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 5);
                //        }
                //    }
                //}

                Company_SaleRep cs = new Company_SaleRep()
                {
                    IsActive = 1,
                    CompanyId = sessiondto.getId(),
                    SaleRepId = sr.Id,
                    Creadted_At = DateTime.Now
                };
                new AdminBL().AddCompany_SaleRep(cs);

                return RedirectToAction("AddSaleRep", new { msg = "Sales representative successfully added." });
            }

            return RedirectToAction("AddSaleRep", new { error = "Please fill all fields carefully." });
        }

        public bool repStatePart(int salerepid, int StateCoverId)
        {
            RepStatePart rs = new RepStatePart()
            {
                SaleRepId = salerepid,
                StateId = StateCoverId,
                IsActive = 1
            };
            int count = new AdminBL().getRepStatePartList().Where(x => x.SaleRepId == rs.SaleRepId && x.StateId == rs.StateId).ToList().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                bool x = new AdminBL().AddRepStatePart(rs);

                return x;
            }
        }

        public ActionResult DeleteSaleRep(int Id)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            Company_SaleRep c = new AdminBL().getCompany_SaleRepById(Id);

            Company_SaleRep cs = new Company_SaleRep()
            {
                Id = c.Id,
                SaleRepId = c.SaleRepId,
                CompanyId = c.CompanyId,
                IsActive = 0,
                Creadted_At = c.Creadted_At
            };
            new AdminBL().UpdateCompany_SaleRep(cs);

            SaleRepresentative obj = new AdminBL().getSaleRepresentativeById(Convert.ToInt32(c.SaleRepId));

            SaleRepresentative sr = new SaleRepresentative()
            {
                Id = obj.Id,
                Name = obj.Name,
                SalesFirmName = obj.SalesFirmName,
                Email = obj.Email,
                EmailReceiveLeads = obj.EmailReceiveLeads,
                PictureUpload = obj.PictureUpload,
                LogoUpload = obj.LogoUpload,
                IsActive = 0
            };
            bool temp = new AdminBL().UpdateSaleRepresentative(sr);

            if (temp)
            {
                List<RepStatePart> repparts = obj.RepStateParts.ToList();

                foreach (RepStatePart r in repparts)
                {
                    RepStatePart rs = new RepStatePart()
                    {
                        Id = r.Id,
                        SaleRepId = r.SaleRepId,
                        StateId = r.StateId,
                        IsActive = 0
                    };
                    new AdminBL().UpdateRepStatePart(rs);
                }

                return RedirectToAction("ListSaleRep", new { msg = "Record successfully deleted." });
            }


            return RedirectToAction("ListSaleRep", new { error = "Please fill all fields carefully." });

        }

        //public ActionResult DeleteSaleRep(int Id)
        //{
        //    if (sessiondto.getRole() != 10)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    SaleRepresentative user = new AdminBL().getSaleRepresentativeList().Where(x => x.Id == Id).FirstOrDefault();
        //    if (user != null)
        //    {
        //        SaleRepresentative existinguser = new SaleRepresentative()
        //        {
        //            Id = user.Id,
        //            Name = user.Name,
        //            Email = user.Email,
        //            EmailReceiveLeads = user.EmailReceiveLeads,
        //            LogoUpload = user.LogoUpload,
        //            PictureUpload = user.PictureUpload,
        //            Creadted_At = user.Creadted_At,
        //            IsActive = user.IsActive
        //        };
        //        existinguser.IsActive = 0;
        //        if (new AdminBL().UpdateSaleRepresentative(existinguser))
        //        {
        //            return RedirectToAction("ListSaleRep", new { msg = "Record Deleted Successfully" });
        //        }
        //        else
        //        {
        //            return RedirectToAction("ListSaleRep", new { msg = "Record Cant Be Deleted" });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("ListSaleRep", new { msg = "Unable to Locate Record" });
        //    }
        //}

        //public ActionResult UpdateSaleRep(int Id, string err, string msg)
        //{
        //    if (sessiondto.getRole() != 10)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    Company_SaleRep obj = new AdminBL().getCompany_SaleRepById(Id);
        //    List<State> allstates = new AdminBL().getStateList();

        //    ViewBag.statescount = allstates.Count();
        //    ViewBag.states = allstates;
        //    ViewBag.Message = msg;
        //    ViewBag.Error = err;

        //    return View(obj);
        //}

        public ActionResult UpdateSaleRep(int Id, string err, string msg)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            Company_SaleRep obj = new AdminBL().getCompany_SaleRepById(Id);
            //SaleRepresentative obj = new AdminBL().getSaleRepresentativeById(Id);
            List<State> allstates = new AdminBL().getStateList();
            ViewBag.SaleRep = obj;
            ViewBag.statescount = allstates.Count();
            ViewBag.states = allstates;
            ViewBag.Message = msg;
            ViewBag.Error = err;
            ViewBag.CoveredStates = allstates;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSaleRep(FormCollection form, int Id = -1, HttpPostedFileBase PictureUpload = null, HttpPostedFileBase LogoUpload = null)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }
            //Company_SaleRep co = new AdminBL().getCompany_SaleRepById(Id);

            SaleRepresentative s = new AdminBL().getSaleRepresentativeById(Convert.ToInt32(Id));
            
            //if (Request.Form["PictureUpload"] == null && Request.Form["LogoUpload"] == null)
            if (PictureUpload == null && LogoUpload == null)
            {
                SaleRepresentative salesrep = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = Request.Form["Name"],
                    SalesFirmName = Request.Form["SalesFirmName"],
                    Email = Request.Form["Email"],
                    EmailReceiveLeads = Request.Form["EmailReceiveLeads"],
                    PictureUpload = s.PictureUpload,
                    LogoUpload = s.LogoUpload,
                    IsActive = s.IsActive

                };

                bool x = new AdminBL().UpdateSaleRepresentative(salesrep);

                if (x)
                {
                    if (Request.Form["StateCover"] != null)
                    {
                        string check = Request.Form["StateCover"];
                        if (check != "Choose a state")
                        {
                            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover"]));
                        }

                    }
                    //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                    //{
                    //    if (Request.Form["StateCover" + i.ToString()] != null)
                    //    {
                    //        if (Request.Form["PartofState" + i.ToString() + "_1"] == "All")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                    //        }
                    //        if (Request.Form["PartofState" + i.ToString() + "_2"] == "East")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                    //        }
                    //        if (Request.Form["PartofState" + i.ToString() + "_3"] == "West")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                    //        }
                    //        if (Request.Form["PartofState" + i.ToString() + "_4"] == "North")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                    //        }
                    //        if (Request.Form["PartofState" + i.ToString() + "_5"] == "South")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 4);
                    //        }
                    //        if (Request.Form["PartofState" + i.ToString() + "_6"] == "Metro")
                    //        {
                    //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 5);
                    //        }
                    //    }
                    //}

                    return RedirectToAction("UpdateSaleRep", new { msg = "You have successfully updated record." });
                }
            }
            //else if (Request.Form["PictureUpload"] != null && Request.Form["LogoUpload"] == null)
            else if (PictureUpload != null && LogoUpload == null)
            {
                SaleRepresentative salesrep = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = Request.Form["Name"],
                    SalesFirmName = Request.Form["SalesFirmName"],
                    Email = Request.Form["Email"],
                    EmailReceiveLeads = Request.Form["EmailReceiveLeads"],
                    LogoUpload = s.LogoUpload,
                    IsActive = s.IsActive

                };
                var file = Request.Files[0];
                if (file != null)
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath("~") + "Content\\SalesRepImages\\");
                        string relativePath = "~/Content/SalesRepImages/" + Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath(relativePath));
                        file.SaveAs(path);
                        salesrep.PictureUpload = relativePath;
                        bool x = new AdminBL().UpdateSaleRepresentative(salesrep);
                        if (x)
                        {
                            string check = Request.Form["StateCover"];
                            if (check != "Choose a state")
                            {
                                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover"]));
                            }
                            //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                            //{
                            //    if (Request.Form["StateCover" + i.ToString()] != null)
                            //    {
                            //        if (Request.Form["PartofState" + i.ToString() + "_1"] == "All")
                            //        {
                            //            for (int j = 0; j < 4; j++)
                            //            {
                            //                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + j);
                            //            }
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_2"] == "East")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_3"] == "West")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_4"] == "North")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_5"] == "South")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                            //        }
                            //    }
                            //}

                            return RedirectToAction("UpdateSaleRep", new { msg = "You have successfully updated record." });
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }

            }
            //else if (Request.Form["PictureUpload"] == null && Request.Form["LogoUpload"] != null)
            else if (PictureUpload == null && LogoUpload != null)
            {
                SaleRepresentative salesrep = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = Request.Form["Name"],
                    SalesFirmName = Request.Form["SalesFirmName"],
                    Email = Request.Form["Email"],
                    EmailReceiveLeads = Request.Form["EmailReceiveLeads"],
                    PictureUpload = s.PictureUpload,
                    IsActive = s.IsActive

                };
                var file = Request.Files[1];
                if (file != null)
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath("~") + "Content\\SalesRepImages\\");
                        string relativePath = "~/Content/SalesRepImages/" + Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath(relativePath));
                        file.SaveAs(path);
                        salesrep.LogoUpload = relativePath;
                        bool x = new AdminBL().UpdateSaleRepresentative(salesrep);
                        if (x)
                        {
                            string check = Request.Form["StateCover"];
                            if (check != "Choose a state")
                            {
                                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover"]));
                            }
                            //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                            //{
                            //    if (Request.Form["StateCover" + i.ToString()] != null)
                            //    {
                            //        if (Request.Form["PartofState" + i.ToString() + "_1"] == "All")
                            //        {
                            //            for (int j = 0; j < 4; j++)
                            //            {
                            //                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + j);
                            //            }
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_2"] == "East")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_3"] == "West")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_4"] == "North")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_5"] == "South")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                            //        }
                            //    }
                            //}

                            return RedirectToAction("UpdateSaleRep", new { msg = "You have successfully updated record." });
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
            }
            else
            {
                SaleRepresentative salesrep = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = Request.Form["Name"],
                    SalesFirmName = Request.Form["SalesFirmName"],
                    Email = Request.Form["Email"],
                    EmailReceiveLeads = Request.Form["EmailReceiveLeads"],
                    PictureUpload = s.PictureUpload,
                    IsActive = s.IsActive

                };
                var file = Request.Files[0];
                var file1 = Request.Files[1];
                if (file != null && file1 != null)
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath("~") + "Content\\SalesRepImages\\");
                        string relativePath = "~/Content/SalesRepImages/" + Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath(relativePath));
                        string relativePath1 = "~/Content/SalesRepImages/" + Path.GetFileName(file1.FileName);
                        string path1 = Path.Combine(Server.MapPath(relativePath));
                        file.SaveAs(path);
                        file1.SaveAs(path1);
                        salesrep.PictureUpload = relativePath;
                        salesrep.LogoUpload = relativePath1;
                        bool x = new AdminBL().UpdateSaleRepresentative(salesrep);
                        if (x)
                        {
                            string check = Request.Form["StateCover"];
                            if (check != "Choose a state")
                            {
                                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover"]));
                            }
                            //for (int i = 0; i <= Convert.ToInt32(Request.Form["count"]); i++)
                            //{
                            //    if (Request.Form["StateCover" + i.ToString()] != null)
                            //    {
                            //        if (Request.Form["PartofState" + i.ToString() + "_1"] == "All")
                            //        {
                            //            for (int j = 0; j < 4; j++)
                            //            {
                            //                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + j);
                            //            }
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_2"] == "East")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]));
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_3"] == "West")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 1);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_4"] == "North")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 2);
                            //        }
                            //        if (Request.Form["PartofState" + i.ToString() + "_5"] == "South")
                            //        {
                            //            repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover" + i.ToString()]) + 3);
                            //        }
                            //    }
                            //}

                            return RedirectToAction("UpdateSaleRep", new { msg = "You have successfully updated record." });
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
            }

            return RedirectToAction("UpdateSaleRep", new { err = "Please fill all fields carefully." });
        }

        public ActionResult DeleteRepState(int Id = -1, int companysalerep = -1)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            RepStatePart rs = new AdminBL().getRepStatePartById(Id);

            RepStatePart r = new RepStatePart()
            {
                Id = rs.Id,
                IsActive = 0,
                SaleRepId = rs.SaleRepId,
                StateId = rs.StateId
            };
            new AdminBL().UpdateRepStatePart(r);

            Company_SaleRep s = new AdminBL().getCompany_SaleRepById(companysalerep);

            return RedirectToAction("UpdateSaleRep", "Company", new { Id = companysalerep, msg = "State is removed from the sales reprentative covered area." });

        }
        #endregion

        #region Campaign

        public ActionResult ListCampaigns()
        {
            if (sessiondto.getRole() == 10)
            {
                int id = sessiondto.getId();
                List<Campaign> campaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == sessiondto.getId()).OrderByDescending(x => x.Id).ToList();

                return View(campaigns);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult AddCampaign(string msg = "", string err = "")
        {
            if (sessiondto.getRole() == 10)
            {
                ViewBag.Message = msg;
                ViewBag.Error = err;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public int validateCampaignName(string Name)
        {
            int count = new AdminBL().getCampaignList().Where(x => x.Name == Name && x.CompanyId == sessiondto.getId()).Count();

            if (Name == "")
            {
                return 2;
            }

            if (count > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        [HttpGet]
        public string GenerateUrl(string campaignname = "")
        {
            if (campaignname != "")
            {
                var charsToRemove = new string[] { " ", "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "[", "}", "]", ";", ":", "'", ",", "<", ".", ">", "/", "?", "'\'", "|" };

                string companyname = sessiondto.getName();

                foreach (var c in charsToRemove)
                {
                    campaignname = campaignname.Replace(c, string.Empty);
                }

                foreach (var c in charsToRemove)
                {
                    companyname = companyname.Replace(c, string.Empty);
                }

                string url = "http://direct2rep.com/d2r/" + companyname + "/"+campaignname;
                //string url = "https://direct2rep.com/d2r/" + companyname + "/"+campaignname;
                //string url = "http://nodlayslahore-001-site3.btempurl.com/d2r/" + companyname + "/"+campaignname;
                //string url = "https://localhost:63900/d2r/" + companyname + "/"+campaignname;

                //Uri uriResult;
                //bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                //    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                int count = new AdminBL().getCampaignList().Where(x => x.Url == url).Count();

                if(count > 0)
                {
                    url = "null";
                }

                return JsonConvert.SerializeObject(url, Formatting.Indented,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            }
            else
            {
                string url = "";

                return JsonConvert.SerializeObject(url, Formatting.Indented,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            }
        }
       

        [HttpPost]
        public ActionResult PostCampaign(Campaign c)
        {
            if (sessiondto.getRole() == 10)
            {                
                c.IsActive = 1;
                c.CompanyId = sessiondto.getId();
                c.Creadted_At = DateTime.Now;

                bool x = new AdminBL().AddCampaign(c);
                if (x)
                {
                    return RedirectToAction("AddCampaign", "Company", new { msg = "New campaign added." });
                }

                return RedirectToAction("AddCampaign", new { err = "Please fill all fields carefully!" });               
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult EditCampaign(string Id, string err, string msg)
        {
            Id = StringCipher.Base64Decode(Id);

            if (sessiondto.getRole() == 10)
            {
                Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(Id));

                ViewBag.Message = msg;
                ViewBag.Error = err;

                return View(c);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult UpdateCampaign(int Id, Campaign camp)
        {
            if (sessiondto.getRole() == 10)
            {
                Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(Id));

                Campaign campaign = new Campaign()
                {
                    Id = c.Id,
                    Name = camp.Name,
                    Url = camp.Url,
                    IsActive = 1,
                    CompanyId = sessiondto.getId(),
                    Creadted_At = c.Creadted_At
                };               

                bool x = new AdminBL().UpdateCampaign(campaign);
                if (x)
                {
                    return RedirectToAction("EditCampaign", new { Id = StringCipher.Base64Encode(Id.ToString()), msg = "Record successfully updated." });
                }

                return RedirectToAction("EditCampaign", new { Id = StringCipher.Base64Encode(Id.ToString()), err = "Please fill all fields carefully!" });
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public ActionResult DeleteCampaign(string Id)
        {
            if (sessiondto.getRole() == 10)
            {
                Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(Id));

                Campaign campaign = new Campaign()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Url = c.Url,
                    IsActive = 0,
                    CompanyId = c.CompanyId,
                    Creadted_At = c.Creadted_At
                };

                new AdminBL().UpdateCampaign(campaign);

                return RedirectToAction("ListCampaigns", "Company");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        #endregion

        #region Reports

        public ActionResult SelectCampaign()
        {
            if (sessiondto.getRole() == 10)
            {
                List<Campaign> campaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == sessiondto.getId()).ToList();
                DateTime enddate = DateTime.Today;
                DateTime startdate = enddate.AddDays(-30);

                ViewBag.startdate = Convert.ToDateTime(startdate);
                ViewBag.enddate = Convert.ToDateTime(enddate);
                ViewBag.campaigns = campaigns.OrderByDescending(x => x.Creadted_At);
                ViewBag.campaignscount = campaigns.Count();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public ActionResult GetCampaignReport(int Campaign_Id, string startdate, string enddate)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }
            List<Visitor> visitors = new List<Visitor>();
            Campaign campaign = new AdminBL().getCampaignById(Campaign_Id);

            if (startdate == "" && enddate == "")
            {
                DateTime enddate2  = DateTime.Today;
                DateTime startdate1 = enddate2.AddDays(-30);

                visitors = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == Campaign_Id && (x.Creadted_At >= Convert.ToDateTime(startdate1) && x.Creadted_At <= Convert.ToDateTime(enddate2.AddDays(1)))).ToList();
                ViewBag.startdate = Convert.ToDateTime(startdate1);
                ViewBag.enddate = Convert.ToDateTime(enddate2);
            }
            else if (startdate != "" && enddate != "")
            {
                visitors = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == Campaign_Id && (x.Creadted_At >= Convert.ToDateTime(startdate) && x.Creadted_At <= Convert.ToDateTime(enddate).AddDays(1))).ToList();

                ViewBag.startdate = Convert.ToDateTime(startdate);
                ViewBag.enddate = Convert.ToDateTime(enddate);
            }
            else
            {
                visitors = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == Campaign_Id && (x.Creadted_At >= Convert.ToDateTime(startdate) || x.Creadted_At <= Convert.ToDateTime(enddate))).ToList();

                if (startdate == "")
                {
                    ViewBag.startdate = null;
                    ViewBag.enddate = Convert.ToDateTime(enddate);
                }
                else if(enddate == "")
                {
                    ViewBag.startdate = Convert.ToDateTime(startdate);
                    ViewBag.enddate = null;
                }               
            }

            List<Campaign> campaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == sessiondto.getId()).ToList();

            ViewBag.campaigns = campaigns.OrderByDescending(x => x.Creadted_At);
            ViewBag.campaign = campaign;

            List<Company_SaleRep> co_sa = new AdminBL().getCompany_SaleRepList().OrderBy(x => x.Creadted_At).ToList();
            ViewBag.co = co_sa;

            List<SaleRepresentative> salerep = new AdminBL().getSaleRepresentativeList().OrderBy(x => x.Creadted_At).ToList();
            ViewBag.salesrep = salerep;

            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }


        public ActionResult SelectSalesRepresentative()
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<Company_SaleRep> compsalreps = new AdminBL().getCompany_SaleRepList().Where(x => x.CompanyId == sessiondto.getId()).ToList();
            List<SaleRepresentative> salesreps = new List<SaleRepresentative>();

            foreach(Company_SaleRep cs in compsalreps)
            {
                SaleRepresentative s = new AdminBL().getSaleRepresentativeById(Convert.ToInt32(cs.SaleRepId));

                SaleRepresentative sale = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = s.Name,
                    SalesFirmName = s.SalesFirmName,
                    Email = s.Email,
                    EmailReceiveLeads = s.EmailReceiveLeads,
                    PictureUpload = s.PictureUpload,
                    LogoUpload = s.LogoUpload,
                    IsActive = s.IsActive,
                    Creadted_At = s.Creadted_At
                };

                salesreps.Add(sale);
            }
            DateTime enddate = DateTime.Today;
            DateTime startdate = enddate.AddDays(-30);

            ViewBag.startdate = Convert.ToDateTime(startdate);
            ViewBag.enddate = Convert.ToDateTime(enddate);
            ViewBag.salesreps = salesreps;
            ViewBag.salesrepscount = salesreps.Count();
            return View();
        }

        [HttpPost]
        public ActionResult GetSaleRepReport(int Salerep_Id, string startdate, string enddate)
        {
            if (sessiondto.getRole() != 10)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<RepStatePart> repstateparts = new AdminBL().getRepStatePartList().Where(x => x.SaleRepId == Salerep_Id).ToList();
            List<Visitor_States> visitorstates = new List<Visitor_States>();

            foreach(RepStatePart r in repstateparts)
            {
                Visitor_States vs = new AdminBL().getVisitor_StatesList().Where(x => x.StateId == r.StateId).LastOrDefault();

                visitorstates.Add(vs);
            }

            List<Visitor> allvisitors = new List<Visitor>();

            foreach(Visitor_States vs in visitorstates)
            {
                Visitor v = new Visitor();
                
                if(vs != null)
                {
                    v = new AdminBL().getVisitorById(Convert.ToInt32(vs.VisitorId));
                }                

                if (v != null)
                {
                    Visitor visitor = new Visitor()
                    {
                        Id = v.Id,
                        Name = v.Name,
                        BusinessName = v.BusinessName,
                        Email = v.Email,
                        Phone = v.Phone,
                        Address = v.Address,
                        City = v.City,
                        ZipCode = v.ZipCode,
                        BestTime = v.BestTime,
                        BestMethod = v.BestMethod,
                        IsActive = v.IsActive,
                        Campaign_Id = v.Campaign_Id,
                        Message = v.Message,
                        Creadted_At = v.Creadted_At
                    };

                    allvisitors.Add(visitor);
                }
            }

            List<Campaign> allcampaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == sessiondto.getId()).ToList();
            List<Visitor> visitors = new List<Visitor>();

            if(allcampaigns.Count() < 1)
            {
                ViewBag.startdate = Convert.ToDateTime(startdate);
                ViewBag.enddate = Convert.ToDateTime(enddate);
            }
            else
            {
                foreach (Campaign c in allcampaigns)
                {
                    foreach (Visitor visi in allvisitors)
                    {

                        if (visi.Campaign_Id == c.Id)
                        {
                            //if (startdate == "" && enddate == "")
                            //{
                            //    DateTime enddate2 = DateTime.Today;
                            //    DateTime startdate1 = enddate2.AddDays(-30);

                            //    if (visi.Creadted_At >= Convert.ToDateTime(startdate1) && visi.Creadted_At <= Convert.ToDateTime(enddate2.AddDays(1)))
                            //    {
                            //        visitors.Add(visi);                                
                            //    }
                            //    ViewBag.startdate = Convert.ToDateTime(startdate1);
                            //    ViewBag.enddate = Convert.ToDateTime(enddate2);
                            //}
                            if (startdate != null && enddate != null)
                            {
                                if (visi.Creadted_At >= Convert.ToDateTime(startdate) && visi.Creadted_At <= Convert.ToDateTime(enddate).AddDays(1))
                                {
                                    visitors.Add(visi);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate);
                                ViewBag.enddate = Convert.ToDateTime(enddate);
                            }
                            else
                            {
                                if (visi.Creadted_At >= Convert.ToDateTime(startdate) || visi.Creadted_At <= Convert.ToDateTime(enddate))
                                {
                                    visitors.Add(visi);
                                    if (startdate == null)
                                    {
                                        ViewBag.startdate = null;
                                        ViewBag.enddate = Convert.ToDateTime(enddate);
                                    }
                                    else if (enddate == null)
                                    {
                                        ViewBag.startdate = Convert.ToDateTime(startdate);
                                        ViewBag.enddate = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            

            SaleRepresentative sr = new AdminBL().getSaleRepresentativeById(Salerep_Id);
            List<Company_SaleRep> compsalreps = new AdminBL().getCompany_SaleRepList().Where(x => x.CompanyId == sessiondto.getId()).ToList();
            List<SaleRepresentative> salesreps = new List<SaleRepresentative>();

            foreach (Company_SaleRep cs in compsalreps)
            {
                SaleRepresentative s = new AdminBL().getSaleRepresentativeById(Convert.ToInt32(cs.SaleRepId));

                SaleRepresentative sale = new SaleRepresentative()
                {
                    Id = s.Id,
                    Name = s.Name,
                    SalesFirmName = s.SalesFirmName,
                    Email = s.Email,
                    EmailReceiveLeads = s.EmailReceiveLeads,
                    PictureUpload = s.PictureUpload,
                    LogoUpload = s.LogoUpload,
                    IsActive = s.IsActive,
                    Creadted_At = s.Creadted_At
                };

                salesreps.Add(sale);
            }

            ViewBag.salesrep = sr;
            ViewBag.salesreps = salesreps;
            ViewBag.salesrepscount = salesreps.Count();
          
            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }
        #endregion
    }
}
