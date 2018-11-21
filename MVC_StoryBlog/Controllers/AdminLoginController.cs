using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class AdminLoginController : Controller
    {
        //
        // GET: /AdminLogin/

        public ActionResult Index()
        {
            Session["adID"] = null;
            Session["adAccount"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string aAccount, string aPwd)
        {
            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var a = db.Administration.FirstOrDefault(n => n.aAccount == aAccount && n.aPwd == aPwd);
                if (a == null)
                {
                    return Content("<script>alert('密码或账号输入错误请注意大小写，请重新输入！');history.go(-1);</script>");
                }
                else
                {
                    Session["adAccount"] = a.aAccount;
                    Session["adID"] = a.ID;
                    return RedirectToAction("index", "admin");
                }
            }



        }

        //定义用户注销方法
        private void adLoginOut()
        {
            Session["adID"] = null;
            Session["adAccount"] = null;
        }

    }
}
