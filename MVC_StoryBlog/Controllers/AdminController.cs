using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;
using System.IO;

namespace MVC_StoryBlog.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        //定义全局变量 接受Session 传值
        private string account = System.Web.HttpContext.Current.Session["adAccount"].ToString();

        public ActionResult Index()
        {
            return View();
        }


        //管理员密码设置
        public ActionResult Setting()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Setting(string oldapwd, string apwd, string reapwd)
        {
            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var ad = db.Administration.Where(a => a.aAccount == account).FirstOrDefault();

                string PassWord = ad.aPwd;

                //判断密码是否为空
                if (oldapwd != "" && apwd != "" && reapwd != "")
                {

                    //判断原始密码是否正确s
                    if (PassWord == oldapwd)
                    {
                        //判断是否包含英文字符
                        char[] ch = new char[apwd.Length];
                        ch = apwd.ToCharArray();
                        bool flag = false;
                        foreach (var item in ch)
                        {
                            if ((item >= 'a' && item < 'z') || (item >= 'A' && item < 'Z'))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            //判断输入新密码的长度
                            if (apwd.Length >= 8 && apwd.Length <= 16)
                            {
                                //判断两次密码是否一致
                                if (apwd == reapwd)
                                {
                                    ad.aPwd = reapwd;

                                    db.SaveChanges();

                                    var obj = new
                                    {

                                        success = "true"

                                    };

                                    return Json(obj, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {
                                    return Content("<script>alert('SORRY!!!两次密码输不一致！');</script>");

                                }
                            }
                            else
                            {
                                return Content("<script>alert('SORRY!!!请输入包含英文字母的8到16位的密码。');</script>");

                            }
                        }

                        else
                        {
                            return Content("<script>alert('SORRY!!!请输入包含英文字母的8到16位的密码。');</script>");

                        }
                    }
                    else
                    {
                        return Content("<script>alert('SORRY!!!原密码输入错误！');</script>");

                    }
                }
                else
                {
                    return Content("<script>alert('SORRY!!!密码不能为空，请输入密码！');</script>");

                }
            }

        }


        //用户管理
        public ActionResult UserManage()
        {
            return View();
        }

        public JsonResult UserManageSel(int page, int rows, string Account)
        {
            if (string.IsNullOrWhiteSpace(Account))
            {
                var user = db.UserInfo.ToList();

                int total = user.Count();

                user = user.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = user.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //账号或者昵称
                var user = db.UserInfo.Where(u => u.Account.Contains(Account) || u.NickName.Contains(Account)).ToList();

                int total = user.Count();

                user = user.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = user.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UserManageDel(List<int?> ID)
        {
            foreach (var i in ID)
            {
                UserInfo ui = db.UserInfo.Where(u => u.ID == i).FirstOrDefault();
                db.UserInfo.Remove(ui);
            }
            db.SaveChanges();
            return Content("OK");
        }


        //文章管理
        public ActionResult ArticleManage()
        {
            return View();
        }

        public JsonResult ArticleManageSel(int page, int rows, string artAccount)
        {
            if (string.IsNullOrWhiteSpace(artAccount))
            {
                var art = (from a in db.ArticleInfo
                           select new ArtHelper
                           {
                               ID = a.ID,
                               artNo = a.artNo,
                               Title = a.Title,
                               uAccount = a.uAccount,
                               artAuthority = a.artAuthority,
                               artHot = a.artHot,
                               artComCnt = a.artComCnt,
                               artDigest = a.artDigest,
                               artCreateTime = a.artCreateTime
                           }).ToList();

                int total = art.Count();

                art = art.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = art.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //账号或者标题
                var art = (from a in db.ArticleInfo
                           select new ArtHelper
                           {
                               ID = a.ID,
                               artNo = a.artNo,
                               Title = a.Title,
                               uAccount = a.uAccount,
                               artAuthority = a.artAuthority,
                               artHot = a.artHot,
                               artComCnt = a.artComCnt,
                               artDigest = a.artDigest,
                               artCreateTime = a.artCreateTime
                           }).Where(a => a.uAccount.Contains(artAccount) || a.Title.Contains(artAccount)).ToList();


                int total = art.Count();

                art = art.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = art.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ArticleManageDel(List<int?> ID)
        {
            foreach (var i in ID)
            {
                ArticleInfo ai = db.ArticleInfo.Where(a => a.ID == i).FirstOrDefault();
                db.ArticleInfo.Remove(ai);
            }
            db.SaveChanges();
            return Content("OK");
        }


        //照片管理
        public ActionResult PictureManage()
        {
            return View();
        }

        public JsonResult PictureManageSel(int page, int rows, string picAccount)
        {
            if (string.IsNullOrWhiteSpace(picAccount))
            {
                var pic = (from p in db.PictureInfo
                           join c in db.PictureClass
                           on p.PicClsID equals c.ID
                           select new PicHelper
                           {
                               ID = p.ID,
                               Name = p.Name,
                               uAccount = p.uAccount,
                               ImgFile = p.ImgFile,
                               picClsTitle = c.picClsTitle,
                               picDescribe = p.picDescribe,
                               picHot = p.picHot,
                               picCreateTime = p.picCreateTime
                           }).ToList();

                int total = pic.Count();

                pic = pic.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = pic.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //账号或者标题
                var pic = (from p in db.PictureInfo
                           join c in db.PictureClass
                           on p.PicClsID equals c.ID
                           select new PicHelper
                           {
                               ID = p.ID,
                               Name = p.Name,
                               uAccount = p.uAccount,
                               ImgFile = p.ImgFile,
                               picClsTitle = c.picClsTitle,
                               picDescribe = p.picDescribe,
                               picHot = p.picHot,
                               picCreateTime = p.picCreateTime
                           }).Where(p => p.uAccount.Contains(picAccount) || p.Name.Contains(picAccount)).ToList();


                int total = pic.Count();

                pic = pic.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = pic.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult PictureManageDel(List<int?> ID)
        {
            foreach (var i in ID)
            {
                PictureInfo pi = db.PictureInfo.Where(a => a.ID == i).FirstOrDefault();
                db.PictureInfo.Remove(pi);
            }
            db.SaveChanges();
            return Content("OK");
        }


        //广告栏管理
        public ActionResult ADClunmsManage()
        {
            return View();
        }

        public JsonResult ADClunmsManageSel(int page, int rows, string adcAccount)
        {
            if (string.IsNullOrWhiteSpace(adcAccount))
            {
                var adc = db.AD_Columns.ToList();

                int total = adc.Count();

                adc = adc.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = adc.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //账号或者标题
                var adc = db.AD_Columns.Where(p => p.adCompany.Contains(adcAccount)).ToList();


                int total = adc.Count();

                adc = adc.Skip((page - 1) * rows).Take(rows).ToList();

                var obj = new
                {
                    total = total,
                    rows = adc.ToArray()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ADClunmsManageDel(List<int?> ID)
        {
            foreach (var i in ID)
            {
                AD_Columns adc = db.AD_Columns.Where(a => a.ID == i).FirstOrDefault();
                db.AD_Columns.Remove(adc);
            }
            db.SaveChanges();
            return Content("OK");
        }

        [HttpPost]
        public ActionResult ADClunmsManageAdd(HttpPostedFileBase adPhoto, string adURL, string adCompany, string adRemarks, string adCompanyTel)
        {
            AD_Columns adc = new AD_Columns();

            FileInfo fi = new FileInfo(adPhoto.FileName);
            string hz = fi.Extension;

            string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

            if (adPhoto.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png" || hz == ".jeg")
            {
                adPhoto.SaveAs(Server.MapPath("~/UploadPicture/ADClunmsPhoto/" + time + hz));

                adc.adPhoto = "/UploadPicture/ADClunmsPhoto/" + time + hz;
                adc.adURL = adURL;
                adc.adCompany = adCompany;
                adc.adRemarks = adRemarks;
                adc.adCompanyTel = adCompanyTel;
                adc.adCreateTime = DateTime.Now;

                db.AD_Columns.Add(adc);
                db.SaveChanges();
                var obj = new
                {
                    success = "true"
                };

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var obj = new
                {
                    no = "true"
                };

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult ADClunmsManageEdit(HttpPostedFileBase adPhoto, string adURL, string adCompany,string adCompanyTel, string adRemarks, int ID)
        {

            FileInfo fi = new FileInfo(adPhoto.FileName);
            string hz = fi.Extension;

            string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

            if (adPhoto.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png" || hz == ".jeg")
            {
                adPhoto.SaveAs(Server.MapPath("~/UploadPicture/ADClunmsPhoto/" + time + hz));

                var adc = db.AD_Columns.Find(ID);
                adc.adPhoto = "/UploadPicture/ADClunmsPhoto/" + time + hz;
                adc.adURL = adURL;
                adc.adCompany = adCompany;
                adc.adRemarks = adRemarks;
                adc.adCompanyTel = adCompanyTel;

                db.SaveChanges();
                var obj = new
                {
                    success = "true"
                };

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var obj = new
                {
                    no = "true"
                };

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
        }


        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
