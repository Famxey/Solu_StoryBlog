using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;
using System.Web.Security;
using System.Drawing;
using System.IO;

namespace MVC_StoryBlog.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        private static string flag = null;

        public ActionResult Index()
        {
            //接受url的值，判断跳转的页面；
            flag = Request["flag"];

            //调用注销的方法
            LoginOut();

            return View();
        }


        [HttpPost]
        public ActionResult Index(UserInfo userinfo, string Account)
        {
            //提交登录信息，判断账号和密码是否合法
            if (ModelState.IsValid && Shujuyanzheng(userinfo.Account, userinfo.PassWord))
            {
                var Luser = db.UserInfo.Where(n => n.Account == Account).FirstOrDefault();

                //跟新数据库的登录时间
                Luser.LoginTime = DateTime.Now;

                db.SaveChanges();

                //定义cookies,接受数据库的值并保存到客户端
                Response.Cookies["User"].Value = Luser.NickName;
                Response.Cookies["Accout"].Value = Luser.Account;
                Response.Cookies["ID"].Value = Luser.ID.ToString();

                //接受前端的值，判断是否七天免登录
                if (Request["ckif"] != null)
                {
                    Response.Cookies["User"].Expires = DateTime.Now.AddDays(7);
                    Response.Cookies["Accout"].Expires = DateTime.Now.AddDays(7);
                    Response.Cookies["ID"].Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    Response.Cookies["User"].Expires = DateTime.Now.AddHours(2);
                    Response.Cookies["Accout"].Expires = DateTime.Now.AddHours(2);
                    Response.Cookies["ID"].Expires = DateTime.Now.AddHours(2);
                }

                //Session["User"] = Server.HtmlEncode(Request.Cookies["User"].Value);
                //Session["Accout"] = Server.HtmlEncode(Request.Cookies["Accout"].Value);
                //Session["ID"] = Server.HtmlEncode(Request.Cookies["ID"].Value);

                //根据接受url的值，判断跳转的页面；
                if (flag == "myp")
                {
                    return RedirectToAction("index", "mypicture");
                }
                else if (flag == "per")
                {
                    return RedirectToAction("index", "personalblog");

                }
                else if (flag == "mya")
                {
                    return RedirectToAction("index", "myarticle");

                }
                else
                {
                    return RedirectToAction("index", "Home");
                }

            }
            else
            {
                ViewBag.yanzheng = "用户名或密码错误";
                return View();
            }

        }

        //定义用户注销方法
        private void LoginOut()
        {
            Session["ID"] = null;
            Session["User"] = null;
            Session["Accout"] = null;

            HttpCookie aCookie;
            string cookName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
        }

        //访问数据库，验证用户登录信息，并且使用Forms验证
        private bool Shujuyanzheng(string Account, string PassWord)
        {
            var u = db.UserInfo.FirstOrDefault(n => n.Account == Account && n.PassWord == PassWord);
            if (u == null)
            {
                return false;
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Account, false);
                return true;
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
