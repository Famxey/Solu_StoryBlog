using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class PersonalBlogController : Controller
    {
        //
        // GET: /PersonalBlog/

        //定义全局变量 接受Session 传值


        //System.Web.HttpContext.Current.Session["Accout"].ToString();

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();



        public ActionResult Index()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            string id = Server.HtmlEncode(Request.Cookies["ID"].Value);
            int userid = Convert.ToInt32(id);

            var user = db.UserInfo.Where(u => u.Account == account);
            ViewBag.picture = user.FirstOrDefault().Picture;
            return View();
        }

        public ActionResult pFrame()
        {
            return View();
        }

        public ActionResult PersonalLeft()
        {
            return View();
        }

        public ActionResult PersonalRight()
        {
            return View();
        }

        //文章管理
        public ActionResult Article()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var ad = from i in db.ArticleInfo
                     join c in db.ArticleClass
                     on i.artClsID equals c.ID
                     where i.uAccount == account
                     select new ArtHelper
                     {
                         ID = i.ID,
                         Title = i.Title,
                         artHot = i.artHot,
                         artComCnt = i.artComCnt,
                         artNo = i.artNo
                     };
            return View(ad.ToList());

        }

        public ActionResult ArtDel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ArtDel(string id)
        {
            ArticleInfo ai = db.ArticleInfo.Where(a => a.artNo == id).FirstOrDefault();
            //删除文章的评论
            var cm = db.ArtComment.Where(c => c.artID == ai.ID).ToList();
            ArtComment acm = new ArtComment();
            foreach (var item in cm)
            {
                acm = db.ArtComment.Find(item.ID);
                db.ArtComment.Remove(acm);
            }

            db.ArticleInfo.Remove(ai);
            db.SaveChanges();
            return RedirectToAction("index", "PersonalBlog");

        }

        private static string artid;
        public ActionResult ArtUpdate()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            var user = db.UserInfo.Where(u => u.Account == account);
            ViewBag.picture = user.FirstOrDefault().Picture;


            artid = Request["id"];

            var ad = from i in db.ArticleInfo
                     join c in db.ArticleClass
                     on i.artClsID equals c.ID
                     where i.uAccount == account && i.artNo == artid
                     select new ArtHelper
                     {
                         ID = i.ID,
                         Title = i.Title,
                         artHot = i.artHot,
                         artComCnt = i.artComCnt,
                         artContent = i.artContent,
                         artDigest = i.artDigest,
                         artClsTitle = c.artClsTitle
                     };
            ViewBag.Title = ad.FirstOrDefault().Title;
            ViewBag.artClsTitle = "--请选择--";
            ViewBag.artDigest = ad.FirstOrDefault().artDigest;
            ViewBag.Content = ad.FirstOrDefault().artContent;

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ArtUpdate(string Title, int? artClsTitle, string artDigest, string artContenter)
        {

            try
            {
                ArticleInfo ai = db.ArticleInfo.Where(a => a.artNo == artid).FirstOrDefault();
                ai.artClsID = (int)artClsTitle;
                ai.Title = Title;
                ai.artDigest = artDigest;
                ai.artContent = artContenter;
                db.SaveChanges();
                return RedirectToAction("index", "PersonalBlog");
            }
            catch (Exception)
            {

                return Content("<script>alert('请选择文章类别！');history.go(-1);</script>");

            }


        }

        //文章分类
        public ActionResult ArtCls()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var ad = from i in db.ArticleClass
                     where i.uAccount == account
                     select new ArtClsHelper { ID = i.ID, artClsTitle = i.artClsTitle };
            return View(ad.ToList());
        }

        public ActionResult ArtClsDel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ArtClsDel(int id)
        {
            ArticleClass ac = db.ArticleClass.Find(id);
            string wzfl = "默认日志";
            bool flag = (wzfl == ac.artClsTitle);
            if (!flag)
            {
                db.ArticleClass.Remove(ac);
                db.SaveChanges();
                return RedirectToAction("index", "PersonalBlog");
            }
            else
            {
                return Content("<script>alert('系统默认日志，不能删除！');history.go(-1);</script>");
            }

        }

        public ActionResult ArtClsUpdate(int? ID, string artClsTitle)
        {

            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            ArticleClass ac = db.ArticleClass.Find(ID);
            string wzfl = "默认日志";
            bool flag = (wzfl == ac.artClsTitle);
            if (!flag)
            {
                ac.artClsTitle = artClsTitle;
            }
            else
            {
                ac.artClsTitle = wzfl;
            }

            ac.uAccount = account;

            db.SaveChanges();

            var obj = new
            {
                ok = "true"
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        //相册管理
        public ActionResult PicCls()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var ad = from p in db.PictureClass
                     where p.uAccount == account
                     select new PicClsHelper
                     {
                         ID = p.ID,
                         picClsTitle = p.picClsTitle,
                         picClsDescribe = p.picClsDescribe,
                         picClsAuthority = p.picClsAuthority,
                         picClsPicCnt = p.picClsPicCnt,
                         uAccount = p.uAccount

                     };
            return View(ad.ToList());
        }
        public ActionResult PicClsDel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PicClsDel(int id)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            PictureClass pcss = db.PictureClass.Find(id);
            string mrxc = "默认相册";
            bool flag = (mrxc == pcss.picClsTitle);
            var piccls = db.PictureClass.Where(pc => pc.picClsTitle == mrxc && pc.uAccount == account).FirstOrDefault();

            if (!flag)
            {

                var picInfo = db.PictureInfo.Where(p => p.PicClsID == id).ToList();
                foreach (var item in picInfo)
                {
                    PictureInfo pi = db.PictureInfo.Find(item.ID);
                    pi.PicClsID = piccls.ID;

                }
                db.PictureClass.Remove(pcss);
                db.SaveChanges();
                //更新默认相册的照片数量
                var pic = db.PictureInfo.Where(p => p.PicClsID == piccls.ID).ToList();
                PictureClass pc = db.PictureClass.Find(piccls.ID);
                pc.picClsPicCnt = pic.Count;
                db.SaveChanges();

                return RedirectToAction("index", "PersonalBlog");
            }
            else
            {
                return Content("<script>alert('系统默认相册，不能删除！');history.go(-1);</script>");
            }

        }

        public ActionResult PicClsUpdate(int? ID, string picClsTitle, int picClsAuthority, string picClsDescribe)
        {

            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

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

        //照片管理
        public ActionResult Picture()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var ad = from p in db.PictureInfo
                     join c in db.PictureClass
                       on p.PicClsID equals c.ID
                     where p.uAccount == account
                     select new PicHelper
                     {
                         ID = p.ID,
                         picClsTitle = c.picClsTitle,
                         PicClsID=p.PicClsID,
                         Name = p.Name,
                         picCreateTime = p.picCreateTime,
                         picDescribe = p.picDescribe
                     };
            return View(ad.ToList());
        }
        public ActionResult PicDel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PicDel(int id)
        {
            var pict = db.PictureInfo.Where(p => p.ID == id).FirstOrDefault();
            var pic = db.PictureInfo.Where(p => p.PicClsID == pict.PicClsID).ToList();

            PictureClass pc = db.PictureClass.Find(pict.PicClsID);
            pc.picClsPicCnt = pic.Count - 1;
            PictureInfo PI = db.PictureInfo.Find(id);

            db.PictureInfo.Remove(PI);
            db.SaveChanges();
            return RedirectToAction("index", "PersonalBlog");
        }
        public ActionResult PicUpdate(int? ID, string Name, int picClsTitle, string picDescribe)
        {

            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            PictureInfo pi = db.PictureInfo.Find(ID);

            pi.Name = Name;
            pi.PicClsID = picClsTitle;
            pi.picDescribe = picDescribe;

            pi.uAccount = account;

            db.SaveChanges();

            var obj = new
            {
                ok = "true"
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        //关注管理
        public ActionResult Attention()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var at = from a in db.AttentionInfo
                     join u in db.UserInfo on
                     a.uAccount equals u.Account
                     where a.uAccount == account
                     select new AttenHelper
                     {
                         ID = a.ID,
                         NickName = (from au in db.UserInfo
                                     where au.Account == a.attenUser
                                     select new
                                     {
                                         au.NickName
                                     }).FirstOrDefault().NickName,
                         attenUser = a.attenUser
                     };
            return View(at.ToList());
        }
        public ActionResult AttentionDel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AttentionDel(int id)
        {
            AttentionInfo atten = db.AttentionInfo.Find(id);
            db.AttentionInfo.Remove(atten);
            db.SaveChanges();
            return RedirectToAction("index", "PersonalBlog");
        }


        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
