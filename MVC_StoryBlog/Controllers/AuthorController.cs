using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class AuthorController : Controller
    {
        //
        // GET: /Author/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index(string uAc)
        {
            ViewBag.uAc = uAc;
            return View();
        }

        //个人信息
        public ActionResult AuthorInfo(string aAuthor)
        {
            var au = db.UserInfo.Where(a => a.Account == aAuthor).FirstOrDefault();
            ViewBag.nikename = au.NickName;
            ViewBag.pic = au.Picture;
            ViewBag.gender = au.Gender;
            ViewBag.describe = au.Describe;
            ViewBag.introduce = au.Introduce;

            return View();
        }

        //作者的文章信息
        public ActionResult artInfo(string aAuthor)
        {
            var aart = from a in db.ArticleInfo
                       where a.uAccount == aAuthor && a.artAuthority == 1
                       select new ArtHelper
                       {
                           Title = a.Title,
                           artHot = a.artHot,
                           artComCnt = a.artComCnt,
                           artNo = a.artNo,
                           ID = a.ID
                       };

            return View(aart.ToList());
        }

        //作者的相册信息
        public ActionResult picClsInfo(string aAuthor)
        {
            var picCls = from p in db.PictureClass
                         where p.uAccount == aAuthor && p.picClsAuthority == 1
                         select new PicClsHelper
                         {
                             ID = p.ID,
                             picClsTitle = p.picClsTitle,
                             picClsDescribe = p.picClsDescribe,
                             CoverFile = p.CoverFile,
                             uAccount = p.uAccount
                         };
            return View(picCls.ToList());
        }

        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
