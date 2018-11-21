using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class StoryClunmsController : Controller
    {
        //
        // GET: /StoryClunms/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            var q = (from i in db.ArticleInfo
                     join u in db.UserInfo
                     on i.uAccount equals u.Account
                     orderby i.artCreateTime descending
                     where i.artAuthority == 1
                     select new ArtHelper
                     {
                         ID = i.ID,
                         Title = i.Title,
                         artDigest = i.artDigest,
                         artNo = i.artNo,
                         artCreateTime = i.artCreateTime,
                         NickName = u.NickName,
                         uAccount = i.uAccount
                     }
               ).Take(100);
            return View(q.ToList());
        }

        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
