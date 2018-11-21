using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class MyArticleController : Controller
    {
        //
        // GET: /MyArticle/


        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            var user = db.UserInfo.Where(u => u.Account == account);
            ViewBag.picture = user.FirstOrDefault().Picture;

            return View();
        }

        //添加文章
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArt(ArticleInfo ai, int artClsTitle, int artAuthority, string artDigest)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            string aiID = DateTime.Now.ToString("yyyyMMddHHmmss");

            ai.artNo = aiID;
            ai.uAccount = account;
            ai.artCreateTime = DateTime.Now;
            ai.artHot = 0;
            ai.artClsID = artClsTitle;
            ai.artAuthority = artAuthority;
            ai.artComCnt = 0;
            ai.artDigest = artDigest.Trim();

            db.ArticleInfo.Add(ai);
            db.SaveChanges();

            return RedirectToAction("SelectArt", "home", new { id = aiID });

        }


        //添加类别
        public ActionResult AddArtCls()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddArtCls(ArticleClass artCls)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            artCls.uAccount = account;
            db.ArticleClass.Add(artCls);
            db.SaveChanges();
            return RedirectToAction("index", "myarticle");
        }


        //查询用户文章分类，绑定数据
        public ActionResult SelectArtCls()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            var artcls = db.ArticleClass.Where(p => p.uAccount == account).Select(n => new
            {
                n.ID,
                n.artClsTitle
            });

            return Json(artcls, JsonRequestBehavior.AllowGet);
        }



        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
