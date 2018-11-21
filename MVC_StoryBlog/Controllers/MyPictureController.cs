using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;
using System.IO;

namespace MVC_StoryBlog.Controllers
{
    public class MyPictureController : Controller
    {
        //
        // GET: /MyPicture/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            string id = Server.HtmlEncode(Request.Cookies["ID"].Value);
            int userid = Convert.ToInt32(id);

            var user = db.UserInfo.Where(u => u.Account == account);
            ViewBag.picture = user.FirstOrDefault().Picture;

            var ad = from p in db.PictureClass
                     where p.uAccount == account
                     select new PicClsHelper
                     {
                         ID = p.ID,
                         picClsTitle = p.picClsTitle,
                         picClsDescribe = p.picClsDescribe,
                         CoverFile = p.CoverFile,
                         uAccount = p.uAccount
                     };
            return View(ad.ToList());

        }

        //查询用户相册，绑定数据
        public ActionResult SelectPicCls()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var piccls = db.PictureClass.Where(p => p.uAccount == account).Select(n => new
            {
                n.ID,
                n.picClsTitle
            });

            return Json(piccls, JsonRequestBehavior.AllowGet);
        }

        //添加相册
        public ActionResult AddPicCls()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddPicCls(HttpPostedFileBase CoverFile, PictureClass picCls)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            if (CoverFile != null)
            {
                FileInfo fi = new FileInfo(CoverFile.FileName);
                string hz = fi.Extension;

                string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                if (CoverFile.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png" || hz == ".jpeg")
                {
                    CoverFile.SaveAs(Server.MapPath("~/UploadPicture/PictureClsCover/" + time + hz));

                    picCls.picClsCreateTime = DateTime.Now;
                    picCls.uAccount = account;

                    picCls.CoverFile = "/UploadPicture/PictureClsCover/" + time + hz;
                    db.PictureClass.Add(picCls);
                    db.SaveChanges();

                }
                else
                {
                    return Content("<script>alert('SORRY!!!上传失败!!!请用.jpg/.jpeg/.gif/.png的照片上传。');history.go(-1);</script>");
                }

            }
            return RedirectToAction("index", "mypicture");
        }
        //修改相册
        [HttpPost]
        public ActionResult PicClsUpdate(int? ID, string picClsTitle, int picClsAuthority, string picClsDescribe, HttpPostedFileBase CoverFile)
        {

            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            if (CoverFile != null)
            {
                FileInfo fi = new FileInfo(CoverFile.FileName);
                string hz = fi.Extension;

                string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                if (CoverFile.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png" || hz == ".jeg")
                {
                    CoverFile.SaveAs(Server.MapPath("~/UploadPicture/PictureClsCover/" + time + hz));

                    PictureClass pc = db.PictureClass.Find(ID);
                    string mrxc = "默认相册";
                    bool flag = (mrxc == pc.picClsTitle);
                    if (!flag)
                    {
                        pc.picClsTitle = picClsTitle;

                    }
                    else
                    {
                        pc.picClsTitle = mrxc;
                    }
                    pc.picClsAuthority = picClsAuthority;
                    pc.picClsDescribe = picClsDescribe;
                    pc.uAccount = account;
                    pc.CoverFile = "/UploadPicture/PictureClsCover/" + time + hz;
                    db.SaveChanges();

                    var obj = new
                    {
                        ok = "true"
                    };

                    return Json(obj, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    ViewBag.cg1 = "SORRY!!!上传失败!!!请用.jpg/.gif/.png的照片上传。";
                    return View();

                }

            }
            else
            {
                PictureClass pc = db.PictureClass.Find(ID);
                string mrxc = "默认相册";
                bool flag = (mrxc == pc.picClsTitle);
                if (!flag)
                {
                    pc.picClsTitle = picClsTitle;

                }
                else
                {
                    pc.picClsTitle = mrxc;
                }
                pc.picClsAuthority = picClsAuthority;
                pc.picClsDescribe = picClsDescribe;
                pc.uAccount = account;

                db.SaveChanges();
                var obj = new
                {
                    ok = "true"
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


        //添加照片
        public ActionResult AddPic()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddPic(HttpPostedFileBase ImgFile, PictureInfo pic, int picClsTitle)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            if (ImgFile != null)
            {
                FileInfo fi = new FileInfo(ImgFile.FileName);
                string hz = fi.Extension;

                string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                if (ImgFile.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png" || hz == ".jpeg")
                {
                    //更新所上传相册的照片量
                    PictureClass pc = db.PictureClass.Find(picClsTitle);
                    pc.picClsPicCnt = pc.picClsPicCnt + 1;

                    ImgFile.SaveAs(Server.MapPath("~/UploadPicture/ImagesFile/" + time + hz));
                    pic.uAccount = account;
                    pic.PicClsID = picClsTitle;
                    pic.picCreateTime = DateTime.Now;
                    pic.picHot = 0;
                    pic.ImgFile = "/UploadPicture/ImagesFile/" + time + hz;
                    db.PictureInfo.Add(pic);
                    db.SaveChanges();
                }
                else
                {
                    return Content("<script>alert('SORRY!!!上传失败!!!请用.jpg/.jpeg/.gif/.png的照片上传。');history.go(-1);</script>");

                }

            }
            return RedirectToAction("index", "mypicture");
        }

        //相册查看照片
        private int clsid;
        public ActionResult SelClsPic(string id, string uA)
        {
            string account;

            try
            {
                account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
                if (account != null)
                {
                    var user = db.UserInfo.Where(u => u.Account == account);
                    ViewBag.picture = user.FirstOrDefault().Picture;
                }
            }
            catch (Exception)
            {

                account = null;
            }

            clsid = int.Parse(id);

            var clsTitle = db.PictureClass.Where(u => u.ID == clsid).FirstOrDefault();
            ViewBag.clsTitle = clsTitle.picClsTitle;

            //给页面传值
            ViewBag.uAcc = uA;

            var ad = from p in db.PictureInfo
                     join
                         c in db.PictureClass on p.PicClsID equals c.ID
                     where p.PicClsID == clsid
                     select new PicHelper { ID = p.ID, Name = p.Name, ImgFile = p.ImgFile, picClsTitle = c.picClsTitle, picDescribe = p.picDescribe };
            return View(ad.ToList());

        }


        //相册里删除照片
        public ActionResult DelPic()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DelPic(int id)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            try
            {

                var pic = db.PictureInfo.Where(p => p.ID == id).FirstOrDefault();
                var pict = db.PictureInfo.Where(p => p.PicClsID == pic.PicClsID).ToList();
                PictureClass pc = db.PictureClass.Find(pic.PicClsID);
                pc.picClsPicCnt = pict.Count - 1;


                PictureInfo pi = db.PictureInfo.Where(p => p.ID == id && p.uAccount == account).First();
                db.PictureInfo.Remove(pi);
                db.SaveChanges();
                //return RedirectToAction("SelClsPic", "MyPicture");
                return View();
            }
            catch (Exception)
            {

                return Content("<script>alert('想删除他人的照片 ？不存在的..');history.go(-1);</script>");
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
