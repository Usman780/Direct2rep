using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Direct2Rep.Models;
using Direct2Rep.BL;
using System.IO;
using Direct2Rep.Helping_Classes;

namespace Direct2Rep.Controllers
{
    public class AdminController : Controller
    {
        SessionDTO sessiondto = new SessionDTO();
       
        // GET: Admin Home Page
        [HttpPost]
        public int validateCompanyName(string Name)
        {
            int nameCount = new AdminBL().getCompanyList().Where(x => x.Name == Name).Count();
            if (nameCount > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public ActionResult Index()
        {
            if (sessiondto.getRole() == 1)
            {
                int companies = new AdminBL().getCompanyList().Count();
                int salesreps = new AdminBL().getSaleRepresentativeList().Count();
                int visitors = new AdminBL().getVisitorList().Count();

                ViewBag.companies = companies;
                ViewBag.salesreps = salesreps;
                ViewBag.visitors = visitors;

                return View();
            }
            else if (sessiondto.getRole() == 10)
            {
                // campaign ka crud banao
                int campaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == sessiondto.getId()).Count();
                int salesreps = new AdminBL().getCompany_SaleRepList().Where(x => x.CompanyId == sessiondto.getId() && x.IsActive == 1).Count();

                ViewBag.campaigns = campaigns;
                ViewBag.salesreps = salesreps;

                return View();
            }
            return RedirectToAction("Login", "Auth");

        }

        #region SalesRep
        public ActionResult AddSaleRep(string err = "", string msg = "")
        {
            if (sessiondto.getRole() != 1)
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
            if (sessiondto.getRole() != 1)
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

        //public ActionResult ListSaleRep(string err, string msg)
        //{
        //    if (sessiondto.getRole() != 1)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    List<Company_SaleRep> lists = new AdminBL().getCompany_SaleRepList().OrderBy(x => x.Creadted_At).ToList();
        //    List<Comp_SaleRepDTOList> co_rp_list = new List<Comp_SaleRepDTOList>();

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

        //        Comp_SaleRepDTOList corpDTO = new Comp_SaleRepDTOList()
        //        {
        //            Id = x.Id,
        //            SaleRepID = x.SaleRepId,
        //            CompanyName = x.Company.Name,
        //            SalesRepNam = x.SaleRepresentative.Name,
        //            SalesRepEmail = x.SaleRepresentative.Email,
        //            SalesRepEmailReceiveLeads = x.SaleRepresentative.EmailReceiveLeads,
        //            SalesRepFirmName = x.SaleRepresentative.SalesFirmName,
        //            SalesRepPictureUpload = x.SaleRepresentative.PictureUpload,
        //            SalesRepLogoUpload = str

        //        };

        //        co_rp_list.Add(corpDTO);
        //        ViewBag.salerep = co_rp_list;
        //    }
        //    return View();
        //}

        public ActionResult ListSaleRep(string err, string msg)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Message = msg;
            ViewBag.Error = err;
            List<SaleRepresentative> lists = new AdminBL().getSaleRepresentativeList().OrderBy(x => x.Creadted_At).ToList();

            List<SaleRepresentative> co_rp_list = new List<SaleRepresentative>();

            string str = "";
            foreach (SaleRepresentative x in lists)
            {
                if (x.LogoUpload != null && x.PictureUpload == null)
                {
                    if (x.LogoUpload.Contains("~"))
                    {
                        x.LogoUpload = x.LogoUpload.Replace('~', ' ');

                        x.LogoUpload.Trim();

                        str = "<td><span>No Picture Found</span> / <a target='_blank' href=" + x.LogoUpload + ">Logo</a> </td>";
                    }
                }
                else if (x.LogoUpload == null && x.PictureUpload != null)
                {
                    if (x.PictureUpload.Contains("~"))
                    {
                        x.PictureUpload = x.PictureUpload.Replace('~', ' ');

                        x.PictureUpload.Trim();

                        str = "<td> <a target='_blank' href=" + x.PictureUpload + ">Picture</a> / <span>No Logo Found</span> </td>";
                    }
                }
                else if (x.LogoUpload != null && x.PictureUpload != null)
                {
                    if (x.PictureUpload.Contains("~") || x.LogoUpload.Contains("~"))
                    {
                        x.PictureUpload = x.PictureUpload.Replace('~', ' ');
                        x.LogoUpload = x.LogoUpload.Replace('~', ' ');

                        x.PictureUpload.Trim();
                        x.LogoUpload.Trim();

                        str = "<td> <a target='_blank' href=" + x.PictureUpload + ">Picture</a> / <a target='_blank' href=" + x.LogoUpload + ">Logo</a> </td>";
                    }
                }
                else
                {
                    str = "No Logo and No Image";
                }

                SaleRepresentative corpDTO = new SaleRepresentative()
                {
                    Id = x.Id,
                    Name = x.Name,
                    SalesFirmName = x.SalesFirmName,
                    Email = x.Email,
                    EmailReceiveLeads = x.EmailReceiveLeads,
                    PictureUpload = x.PictureUpload,
                    LogoUpload = x.LogoUpload,
                    IsActive = x.IsActive,
                    Creadted_At = x.Creadted_At
                };

                co_rp_list.Add(corpDTO);
                ViewBag.salerep = co_rp_list;
            }
            return View();
        }

        public ActionResult DeleteSaleRep(int Id)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            SaleRepresentative obj = new AdminBL().getSaleRepresentativeById(Id);

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
        //    if (sessiondto.getRole() != 1)
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    Company_SaleRep obj = new AdminBL().getCompany_SaleRepById(Id);
        //    if(obj != null)
        //    {
        //        Company_SaleRep sr = new Company_SaleRep()
        //        {
        //            Id = obj.Id,
        //            CompanyId = obj.CompanyId,
        //            SaleRepId = obj.SaleRepId,
        //        };
        //        sr.IsActive = 0;
        //        if (new AdminBL().UpdateCompany_SaleRep(sr))
        //        {
        //            return RedirectToAction("ListSaleRep", "Admin", new { msg = "User Deleted Successfully" });
        //        }
        //        else
        //        {
        //            return RedirectToAction("ListSaleRep", "Admin", new { msg = "User Cant Be Deleted" });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("ListSaleRep", "Admin", new { msg = "Unable to Locate User" });
        //    }
            
        //}

        public ActionResult UpdateSaleRep(int Id, string err, string msg)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            //Company_SaleRep obj = new AdminBL().getCompany_SaleRepById(Id);
            SaleRepresentative obj = new AdminBL().getSaleRepresentativeById(Id);
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
        public ActionResult PostUpdateSaleRep(FormCollection form, HttpPostedFileBase PictureUpload = null, HttpPostedFileBase LogoUpload = null)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            

            SaleRepresentative s = new AdminBL().getSaleRepresentativeById(Convert.ToInt32(Request.Form["Id"]));
            int count = Request.ContentLength;
            if(count > 0)
            {
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

                        if (Request.Form["StateCover0"] != null)
                        {
                            //var w = Request.Form["StateCover0"];
                            //repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover0"]));
                            string check = Request.Form["StateCover0"];
                            if (check != "Choose a state")
                            {
                                repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover0"]));
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

                        return RedirectToAction("ListSaleRep", "Admin", new { msg = "You have successfully updated record." });
                    }
                }
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
                                string check = Request.Form["StateCover0"];
                                if (check != "Choose a state")
                                {
                                    repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover0"]));
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

                                return RedirectToAction("ListSaleRep", "Admin", new { msg = "You have successfully updated record." });
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }

                }
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
                                string check = Request.Form["StateCover0"];
                                if (check != "Choose a state")
                                {
                                    repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover0"]));
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

                                return RedirectToAction("ListSaleRep", "Admin", new { msg = "You have successfully updated record." });
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
                                string check = Request.Form["StateCover0"];
                                if (check != "Choose a state")
                                {
                                    repStatePart(salesrep.Id, Convert.ToInt32(Request.Form["StateCover0"]));
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

                                return RedirectToAction("ListSaleRep", "Admin", new { msg = "You have successfully updated record." });
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                }
            }

            return RedirectToAction("ListSaleRep","Admin", new { err = "Please fill all fields carefully." });
        }

        public ActionResult DeleteRepState(int Id, int salerep)
        {
            if (sessiondto.getRole() != 1)
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

            SaleRepresentative s = new AdminBL().getSaleRepresentativeById(salerep);

            return RedirectToAction("UpdateSaleRep", "Admin", new { Id = salerep, msg = "State is removed from the sales reprentative covered area." });

        }
        #endregion

        #region Company
        public ActionResult AddCompany(string msg = "", string err = "")
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Message = msg;
            ViewBag.Error = err;
            return View();
        }

        [HttpPost]
        public ActionResult PostCompany(Company obj, FormCollection form)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                string ext = System.IO.Path.GetExtension(System.IO.Path.GetFileName(file.FileName));
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".JPG" || ext == ".JPEG" || ext == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    if (height > 600 || width > 600)
                    {
                        return RedirectToAction("AddCompany", new { err = "Your Image Width x Height Should Be Less Than or Equal To 600 x 600" });
                    }
                    Directory.CreateDirectory(Server.MapPath("~") + "Content\\CompanyImages\\");
                    string relativePath = "~/Content/CompanyImages/" + Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath(relativePath));
                    file.SaveAs(path);
                    obj.Logo = relativePath;
                }
                else
                {
                    return RedirectToAction("AddCompany", new { err = "Your Image Type Must be JPG, JPEG and PNG" });
                }
            }
            obj.IsActive = 1;
            obj.Creadted_At = DateTime.Now;
            bool x = new AdminBL().AddCompany(obj);
            if (x)
            {
                return RedirectToAction("AddCompany", new { msg = "New company / organization added." });
            }

            return RedirectToAction("AddCompany", new { err = "Please fill all fields carefully!" });
        }

        public ActionResult ListCompany(string err, string msg)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Message = msg;
            ViewBag.Error = err;
            //List<Company_SaleRep> comp = new AdminBL().getCompany_SaleRepList().ToList();
            List<Company> comp = new AdminBL().getCompanyList().OrderBy(x => x.Name).ToList();
            ViewBag.data = comp;
            return View();
        }

        public ActionResult DeleteCompany(int Id)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            Company obj = new AdminBL().getCompanyById(Id);
            Company cp = new Company()
            {
                Id = obj.Id,
                Name = obj.Name,
                Email = obj.Email,
                Phone = obj.Phone,
                Website = obj.Website,
                Logo = obj.Logo,
                Password = obj.Password,
                IsActive = 0
                };
            bool temp = new AdminBL().UpdateCompany(cp);
            if (temp)
                return RedirectToAction("ListCompany", new { msg = "Record successfully deleted." });
            return RedirectToAction("ListCompany", new { error = "Please fill all fields carefully." });

        }

        public ActionResult UpdateCompany(int Id, string err, string msg)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Message = msg;
            ViewBag.Error = err;
            Company obj = new AdminBL().getCompanyById(Id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult PostUpdateCompany(Company obj, FormCollection form)
        {
            if (sessiondto.getName() == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            Company c = new AdminBL().getCompanyById(obj.Id);
            if (sessiondto.getRole() == 1)
            {
                if (obj.Logo == null)
                {
                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = obj.Name,
                        Phone = obj.Phone,
                        Email = obj.Email,
                        Website = obj.Website,
                        Logo = c.Logo,
                        Password = obj.Password,
                        IsActive = c.IsActive
                    };
                    bool x = new AdminBL().UpdateCompany(company);
                    if (x)
                    {
                        return RedirectToAction("ListCompany", "Admin", new { msg = "Record successfully updated." });
                    }

                }
                else
                {
                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = obj.Name,
                        Phone = obj.Phone,
                        Email = obj.Email,
                        Website = obj.Website,
                        Password = obj.Password,
                        IsActive = c.IsActive
                    };
                    var file = Request.Files[0];
                    if (file != null)
                        try
                        {
                            string ext = System.IO.Path.GetExtension(System.IO.Path.GetFileName(file.FileName));
                            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".JPG" || ext == ".JPEG" || ext == ".PNG")
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                                int height = img.Height;
                                int width = img.Width;
                                if (height > 600 || width > 600)
                                {
                                    return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, err = "Your Image Width x Height Should Be Less Than or Equal To 600 x 600" });
                                }
                                Directory.CreateDirectory(Server.MapPath("~") + "Content\\CompanyImages\\");
                                string relativePath = "~/Content/CompanyImages/" + Path.GetFileName(file.FileName);
                                string path = Path.Combine(Server.MapPath(relativePath));
                                file.SaveAs(path);
                                company.Logo = relativePath;
                                bool x = new AdminBL().UpdateCompany(company);
                                if (x)
                                {
                                    return RedirectToAction("ListCompany", "Admin", new { msg = "Record successfully updated." });
                                }
                            }
                            else
                            {
                                return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, err = "Your Image Type Must be JPG, JPEG and PNG" });
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                }
            }
            else
            {
                if (obj.Logo == null)
                {
                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = obj.Name,
                        Phone = obj.Phone,
                        Email = obj.Email,
                        Website = obj.Website,
                        Logo = c.Logo,
                        Password = obj.Password,
                        IsActive = c.IsActive
                    };
                    bool x = new AdminBL().UpdateCompany(company);
                    if (x)
                    {
                        return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, msg = "Record successfully updated." });
                    }
                }
                else
                {
                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = obj.Name,
                        Phone = obj.Phone,
                        Email = obj.Email,
                        Website = obj.Website,
                        Password = obj.Password,
                        IsActive = c.IsActive
                    };
                    var file = Request.Files[0];
                    if (file != null)
                        try
                        {
                            string ext = System.IO.Path.GetExtension(System.IO.Path.GetFileName(file.FileName));
                            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".JPG" || ext == ".JPEG" || ext == ".PNG")
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                                int height = img.Height;
                                int width = img.Width;
                                if (height > 600 || width > 600)
                                {
                                    return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, err = "Your Image Width x Height Should Be Less Than or Equal To 600 x 600" });
                                }
                                Directory.CreateDirectory(Server.MapPath("~") + "Content\\CompanyImages\\");
                                string relativePath = "~/Content/CompanyImages/" + Path.GetFileName(file.FileName);
                                string path = Path.Combine(Server.MapPath(relativePath));
                                file.SaveAs(path);
                                company.Logo = relativePath;
                                bool x = new AdminBL().UpdateCompany(company);
                                if (x)
                                {
                                    return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, msg = "Record successfully updated." });
                                }
                            }
                            else
                            {
                                return RedirectToAction("UpdateCompany", "Admin", new { Id = c.Id, err = "Your Image Type Must be JPG, JPEG and PNG" });
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                }
            }

            return RedirectToAction("ListCompany", new { err = "Please fill all fields carefully!" });

        }
        #endregion

        #region Company_Sales
        public ActionResult ListSalesRepCompany(string err, string msg)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }
            List<Company_SaleRep> compsales = new AdminBL().getCompany_SaleRepList().OrderByDescending(x => x.Id).ToList();

            ViewBag.compsales = compsales;
            return View(compsales);
        }
        
        public ActionResult AssignRepToCompany(string msg = "", string err = "")
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<Company> companies = new AdminBL().getCompanyList();
            List<SaleRepresentative> salereps = new AdminBL().getSaleRepresentativeList();

            ViewBag.Message = msg;
            ViewBag.Error = err;
            ViewBag.companies = companies;
            ViewBag.salereps = salereps;

            return View();
        }

        [HttpPost]
        public ActionResult AssignRepToCompany(int SaleRepId, int CompanyId)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            Company_SaleRep cs = new AdminBL().getCompany_SaleRepList().Where(x => x.SaleRepId == SaleRepId && x.CompanyId == CompanyId).FirstOrDefault();

            if (cs == null)
            {
                Company_SaleRep compsale = new Company_SaleRep()
                {
                    SaleRepId = SaleRepId,
                    CompanyId = CompanyId,
                    Creadted_At = DateTime.Now,
                    IsActive = 1
                };
                bool x = new AdminBL().AddCompany_SaleRep(compsale);
                if (x)
                {
                    return RedirectToAction("AssignRepToCompany", "Admin", new { msg = "Record added successfully." });
                }
                return RedirectToAction("AssignRepToCompany", "Admin", new { msg = "Error in assigning." });
            }
            else
            {
                return RedirectToAction("AssignRepToCompany", "Admin", new { msg = "This sales rep is already assigned to this company." });
            }
        }

        public ActionResult DeleteCompanySaleRep(int Id)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            new AdminBL().DeleteCompany_SaleRep(Id);

            return RedirectToAction("ListSalesRepCompany");

        }
        #endregion

        #region Reports
        public ActionResult SelectCompany()
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<Company> companies = new AdminBL().getCompanyList();
            DateTime enddate = DateTime.Now.Date;
            DateTime startdate = enddate.AddDays(-30);
            
            ViewBag.startdate = Convert.ToDateTime(startdate);
            ViewBag.enddate = Convert.ToDateTime(enddate);
            ViewBag.companies = companies;
            ViewBag.companiescount = companies.Count();
            return View();
        }

        [HttpPost]
        public ActionResult GetCompanyReport(int Company_Id, string startdate, string enddate)
        {
            string startdateChange = "";
            string enddateChange = "";

            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            Company company = new AdminBL().getCompanyById(Company_Id);
            List<Campaign> campaigns = new AdminBL().getCampaignList().Where(x => x.CompanyId == Company_Id).ToList();
            List<Visitor> visitors = new List<Visitor>();

            string[] startDateArray = startdate.Split('-');
            if (startDateArray[0].Count() == 2)
            {
                startdateChange = startDateArray[2] + "-" + startDateArray[1] + "-" + startDateArray[0];
            }
            else
            {
                startdateChange = startdate;
            }
            string[] endDateArray = startdate.Split('-');
            if (endDateArray[0].Count() == 2)
            {
                enddateChange = startDateArray[2] + "-" + startDateArray[1] + "-" + startDateArray[0];
            }
            else
            {
                enddateChange = enddate;
            }
            if (campaigns.Count() > 0)
            {
                foreach(Campaign c in campaigns)
                {
                    List<Visitor> vs = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == c.Id).ToList();

                    if(vs.Count() > 0)
                    {
                        foreach(Visitor v in vs)
                        {
                            if (startdate == "" && enddate == "")
                            {
                                DateTime enddate2  = DateTime.Today;
                                DateTime startdate1 = enddate2.AddDays(-30);

                                if (v.Creadted_At >= Convert.ToDateTime(startdate1) && v.Creadted_At <= Convert.ToDateTime(enddate2.AddDays(1)))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate1);
                                ViewBag.enddate = Convert.ToDateTime(enddate2);
                            }
                            else if (startdate != "" && enddate != "")
                            {
                                //change here
                                if (v.Creadted_At >= Convert.ToDateTime(startdateChange) && v.Creadted_At <= Convert.ToDateTime(enddateChange).AddDays(1))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                                ViewBag.enddate = Convert.ToDateTime(enddateChange);
                            }
                            else
                            {
                                //change here
                                if (v.Creadted_At >= Convert.ToDateTime(startdateChange) || v.Creadted_At <= Convert.ToDateTime(enddateChange))
                                {
                                    visitors.Add(v);
                                }
                                if (startdate == "")
                                {
                                    ViewBag.startdate = null;
                                    ViewBag.enddate = Convert.ToDateTime(enddateChange);
                                }
                                else if (enddate == "")
                                {
                                    //change here
                                    ViewBag.startdate = Convert.ToDateTime(startdateChange);
                                    ViewBag.enddate = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.startdate = Convert.ToDateTime(startdateChange);
                        ViewBag.enddate = Convert.ToDateTime(enddateChange);
                    }
                }
            }
            else
            {
                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                ViewBag.enddate = Convert.ToDateTime(enddateChange);
            }
            List<Company> companies = new AdminBL().getCompanyList();

            ViewBag.companies = companies;
            ViewBag.company = company;
            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }

        public ActionResult SelectSalesRepresentative()
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();
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
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            SaleRepresentative salerep = new AdminBL().getSaleRepresentativeById(Salerep_Id);

            List<Company_SaleRep> companysalereps = new AdminBL().getCompany_SaleRepList().Where(x => x.SaleRepId == Salerep_Id).ToList();
            List<Company> companies = new List<Company>();
            List<Campaign> campaigns = new List<Campaign>();
            string startdateChange = "";
            string enddateChange = "";

            string[] startDateArray = startdate.Split('-');
            if (startDateArray[0].Count() == 2)
            {
                startdateChange = startDateArray[2] + "-" + startDateArray[1] + "-" + startDateArray[0];
            }
            else
            {
                startdateChange = startdate;
            }
            string[] endDateArray = startdate.Split('-');
            if (endDateArray[0].Count() == 2)
            {
                enddateChange = startDateArray[2] + "-" + startDateArray[1] + "-" + startDateArray[0];
            }
            else
            {
                enddateChange = enddate;
            }
            if (companysalereps.Count() > 0)
            {
                foreach(Company_SaleRep cs in companysalereps)
                {
                    
                    Company c = new AdminBL().getCompanyById(Convert.ToInt32(cs.CompanyId));
                    if (c != null)
                    {
                        Company company = new Company()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Logo = c.Logo,
                            Website = c.Website,
                            Phone = c.Phone,
                            Email = c.Email,
                            IsActive = c.IsActive,
                            Password = c.Password,
                            Creadted_At = c.Creadted_At
                        };
                        companies.Add(company);
                    }
                }
            }
            else
            {
                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                ViewBag.enddate = Convert.ToDateTime(enddateChange);
            }

            if (companies.Count() > 0)
            {
                foreach (Company cs in companies)
                {
                    Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(cs.Id));

                    if(c != null)
                    {
                        Campaign campaign = new Campaign()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Url = c.Url,
                            IsActive = c.IsActive,
                            CompanyId = c.CompanyId,
                            Creadted_At = c.Creadted_At
                        };
                        campaigns.Add(campaign);
                    }                    
                }
            }
            else
            {
                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                ViewBag.enddate = Convert.ToDateTime(enddateChange);
            }

            List<Visitor> visitors = new List<Visitor>();

            if (campaigns.Count() > 0)
            {
                foreach (Campaign c in campaigns)
                {
                    List<Visitor> vs = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == c.Id).ToList();

                    if (vs.Count() > 0)
                    {
                        foreach (Visitor v in vs)
                        {
                            if (startdate == "" && enddate == "")
                            {
                                DateTime startdate1 = DateTime.Today;
                                DateTime enddate2 = startdate1.AddDays(-30);

                                if (v.Creadted_At >= Convert.ToDateTime(startdate1) && v.Creadted_At <= Convert.ToDateTime(enddate2))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate1);
                                ViewBag.enddate = Convert.ToDateTime(enddate2);
                            }
                            else if (startdate != "" && enddate != "")
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdateChange) && v.Creadted_At <= Convert.ToDateTime(enddateChange).AddDays(1))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                                ViewBag.enddate = Convert.ToDateTime(enddateChange);
                            }
                            else
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdateChange) || v.Creadted_At <= Convert.ToDateTime(enddateChange))
                                {
                                    visitors.Add(v);
                                }
                                if (startdate == "")
                                {
                                    ViewBag.startdate = null;
                                    ViewBag.enddate = Convert.ToDateTime(enddateChange);
                                }
                                else if (enddate == "")
                                {
                                    ViewBag.startdate = Convert.ToDateTime(startdateChange);
                                    ViewBag.enddate = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.startdate = Convert.ToDateTime(startdateChange);
                        ViewBag.enddate = Convert.ToDateTime(enddateChange);
                    }
                }
            }
            else
            {
                ViewBag.startdate = Convert.ToDateTime(startdateChange);
                ViewBag.enddate = Convert.ToDateTime(enddateChange);
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();

            ViewBag.salesreps = salesreps;
            ViewBag.salerep = salerep;
            ViewBag.salesrepscount = salesreps.Count();     
            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }

        public ActionResult SelectRepresentativeFirm()
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();
            DateTime enddate = DateTime.Today;
            DateTime startdate = enddate.AddDays(-30);

            ViewBag.startdate = Convert.ToDateTime(startdate);
            ViewBag.enddate = Convert.ToDateTime(enddate);
            ViewBag.salesreps = salesreps;
            ViewBag.salesrepscount = salesreps.Count();
            return View();
        }

        public ActionResult GetFirmReport(int Salrep_Id, string startdate, string enddate)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            SaleRepresentative salerep = new AdminBL().getSaleRepresentativeById(Salrep_Id);

            List<Company_SaleRep> companysalereps = new AdminBL().getCompany_SaleRepList().Where(x => x.SaleRepId == Salrep_Id).ToList();
            List<Company> companies = new List<Company>();
            List<Campaign> campaigns = new List<Campaign>();

            if (companysalereps.Count() > 0)
            {
                foreach (Company_SaleRep cs in companysalereps)
                {
                    Company c = new AdminBL().getCompanyById(Convert.ToInt32(cs.CompanyId));

                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Logo = c.Logo,
                        Website = c.Website,
                        Phone = c.Phone,
                        Email = c.Email,
                        IsActive = c.IsActive,
                        Password = c.Password,
                        Creadted_At = c.Creadted_At
                    };
                    companies.Add(company);
                }
            }

            if (companies.Count() > 0)
            {
                foreach (Company cs in companies)
                {
                    Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(cs.Id));

                    if (c != null)
                    {
                        Campaign campaign = new Campaign()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Url = c.Url,
                            IsActive = c.IsActive,
                            CompanyId = c.CompanyId,
                            Creadted_At = c.Creadted_At
                        };
                        campaigns.Add(campaign);
                    }
                }
            }

            List<Visitor> visitors = new List<Visitor>();

            if (campaigns.Count() > 0)
            {
                foreach (Campaign c in campaigns)
                {
                    List<Visitor> vs = new AdminBL().getVisitorList().Where(x => x.Campaign_Id == c.Id).ToList();

                    if (vs.Count() > 0)
                    {
                        foreach (Visitor v in vs)
                        {
                            if (startdate == "" && enddate == "")
                            {
                                DateTime startdate1 = DateTime.Today;
                                DateTime enddate2 = startdate1.AddDays(-30);

                                if (v.Creadted_At >= Convert.ToDateTime(startdate1) && v.Creadted_At <= Convert.ToDateTime(enddate2))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate1);
                                ViewBag.enddate = Convert.ToDateTime(enddate2);
                            }
                            else if (startdate != "" && enddate != "")
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdate) && v.Creadted_At <= Convert.ToDateTime(enddate).AddDays(1))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate);
                                ViewBag.enddate = Convert.ToDateTime(enddate);
                            }
                            else
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdate) || v.Creadted_At <= Convert.ToDateTime(enddate))
                                {
                                    visitors.Add(v);
                                }
                                if (startdate == "")
                                {
                                    ViewBag.startdate = null;
                                    ViewBag.enddate = Convert.ToDateTime(enddate);
                                }
                                else if (enddate == "")
                                {
                                    ViewBag.startdate = Convert.ToDateTime(startdate);
                                    ViewBag.enddate = null;
                                }
                            }
                        }
                    }
                }
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();

            ViewBag.salesreps = salesreps;
            ViewBag.salerep = salerep;
            ViewBag.salesrepscount = salesreps.Count();
            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }

        public ActionResult SelectDate()
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();
            DateTime enddate = DateTime.Today;
            DateTime startdate = enddate.AddDays(-30);

            ViewBag.startdate = Convert.ToDateTime(startdate);
            ViewBag.enddate = Convert.ToDateTime(enddate);
            ViewBag.salesreps = salesreps;
            ViewBag.salesrepscount = salesreps.Count();
            return View();
        }

        public ActionResult GetDateReport(int Salrep_Date_Id, string startdate, string enddate)
        {
            if (sessiondto.getRole() != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            SaleRepresentative salerep = new AdminBL().getSaleRepresentativeById(Salrep_Date_Id);

            List<Company_SaleRep> companysalereps = new AdminBL().getCompany_SaleRepList().Where(x => x.SaleRepId == Salrep_Date_Id).ToList();
            List<Company> companies = new List<Company>();
            List<Campaign> campaigns = new List<Campaign>();

            if (companysalereps.Count() > 0)
            {
                foreach (Company_SaleRep cs in companysalereps)
                {
                    Company c = new AdminBL().getCompanyById(Convert.ToInt32(cs.CompanyId));

                    Company company = new Company()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Logo = c.Logo,
                        Website = c.Website,
                        Phone = c.Phone,
                        Email = c.Email,
                        IsActive = c.IsActive,
                        Password = c.Password,
                        Creadted_At = c.Creadted_At
                    };
                    companies.Add(company);
                }
            }

            if (companies.Count() > 0)
            {
                foreach (Company cs in companies)
                {
                    Campaign c = new AdminBL().getCampaignById(Convert.ToInt32(cs.Id));

                    if (c != null)
                    {
                        Campaign campaign = new Campaign()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Url = c.Url,
                            IsActive = c.IsActive,
                            CompanyId = c.CompanyId,
                            Creadted_At = c.Creadted_At
                        };
                        campaigns.Add(campaign);
                    }
                }
            }

            List<Visitor> visitors = new List<Visitor>();

            if (campaigns.Count() > 0)
            {
                foreach (Campaign c in campaigns)
                {
                    List<Visitor> vs = new AdminBL().getVisitorList().OrderBy(x => x.Creadted_At).ToList();

                    if (vs.Count() > 0)
                    {
                        foreach (Visitor v in vs)
                        {
                            if (startdate == "" && enddate == "")
                            {
                                DateTime startdate1 = DateTime.Today;
                                DateTime enddate2 = startdate1.AddDays(-30);

                                if (v.Creadted_At >= Convert.ToDateTime(startdate1) && v.Creadted_At <= Convert.ToDateTime(enddate2))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate1);
                                ViewBag.enddate = Convert.ToDateTime(enddate2);
                            }
                            else if (startdate != "" && enddate != "")
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdate) && v.Creadted_At <= Convert.ToDateTime(enddate).AddDays(1))
                                {
                                    visitors.Add(v);
                                }
                                ViewBag.startdate = Convert.ToDateTime(startdate);
                                ViewBag.enddate = Convert.ToDateTime(enddate);
                            }
                            else
                            {
                                if (v.Creadted_At >= Convert.ToDateTime(startdate) || v.Creadted_At <= Convert.ToDateTime(enddate))
                                {
                                    visitors.Add(v);
                                }
                                if (startdate == "")
                                {
                                    ViewBag.startdate = null;
                                    ViewBag.enddate = Convert.ToDateTime(enddate);
                                }
                                else if (enddate == "")
                                {
                                    ViewBag.startdate = Convert.ToDateTime(startdate);
                                    ViewBag.enddate = null;
                                }
                            }
                        }
                    }
                }
            }

            List<SaleRepresentative> salesreps = new AdminBL().getSaleRepresentativeList();

            ViewBag.salesreps = salesreps;
            ViewBag.salerep = salerep;
            ViewBag.salesrepscount = salesreps.Count();
            ViewBag.visitors = visitors.OrderBy(x => x.Name);
            ViewBag.visitorscount = visitors.Count();

            return View();
        }

        #endregion
    }
}