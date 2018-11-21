using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/


        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        //添加文章评论
        public ActionResult AddArtcm(ArtComment cm, string uAccout)
        {

            if (uAccout != "")
            {
                cm.uAccount = uAccout;
                cm.artCmCreateTime = DateTime.Now;
                db.ArtComment.Add(cm);
                db.SaveChanges();
            }
            else
            {
                cm.artCmCreateTime = DateTime.Now;
                cm.uAccount = "cyan";
                db.ArtComment.Add(cm);
                db.SaveChanges();
            }

            var acm = db.ArtComment.Where(c => c.artID == cm.artID).ToList();
            int artcmcn = acm.Count;
            ArticleInfo ai = db.ArticleInfo.Find(cm.artID);
            ai.artComCnt = artcmcn;
            db.SaveChanges();

            var obj = new
            {
                ok = "true"
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
