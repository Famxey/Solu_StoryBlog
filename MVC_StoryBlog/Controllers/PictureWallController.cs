using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class PictureWallController : Controller
    {
        //
        // GET: /PictureWall/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            var q = (from i in db.PictureInfo
                     join c in db.PictureClass on i.PicClsID equals c.ID
                     orderby i.picCreateTime descending
                     where c.picClsAuthority == 1
                     select new PicHelper { ID = i.ID, picDescribe = i.picDescribe, Name = i.Name, ImgFile = i.ImgFile }).Take(96).ToList();

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
