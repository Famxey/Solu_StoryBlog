using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class AttentionUserController : Controller
    {
        //
        // GET: /AttentionUser/

        public ActionResult Index()
        {
            return View();
        }

        //添加关注作者
        public ActionResult Add(string attenUser)
        {

            try
            {
                using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
                {
                    string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

                    //检测数据库是否有改数据，没有则添加
                    var at = db.AttentionInfo.Where(a => a.attenUser == attenUser).FirstOrDefault();

                    if (at == null)
                    {
                        AttentionInfo atten = new AttentionInfo();
                        atten.attenUser = attenUser;
                        atten.uAccount = account;
                        db.AttentionInfo.Add(atten);
                        db.SaveChanges();

                        var obj = new
                        {
                            ok = "true"
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
            }
            catch (Exception)
            {
                var obj = new
                {
                    dl = "true"
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }


        }
    }
}
