using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Drawing;
using System.IO;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserInfo userinfo, string NickName, string Account, string PassWord, string PassWord1, string name)
        {
            //判断验证码
            if (name != Session["ValidateNum"].ToString())
            {
                return Content("<script>alert('验证码错误请注意大小写，请重新输入！');history.go(-1);</script>");
            }
            else
            {
                userinfo.NickName = NickName;
                userinfo.Account = Account;
                userinfo.PassWord = PassWord;
                userinfo.Gender = "保密";
                userinfo.Describe = "这个人很懒，什么都没有...";
                userinfo.Introduce = "这个人很懒，什么都没有...";
                userinfo.CreateTime = DateTime.Now;
                db.UserInfo.Add(userinfo);

                //创建默认的日志
                ArticleClass ac = new ArticleClass();
                ac.artClsTitle = "默认日志";
                ac.uAccount = Account;
                db.ArticleClass.Add(ac);

                //创建默认的相册
                PictureClass pc = new PictureClass();
                //pc.CoverFile = "/UploadPicture/ImagesFile/20170921061035749421.jpg";
                pc.picClsTitle = "默认相册";
                pc.uAccount = Account;
                pc.picClsAuthority = 0;
                pc.picClsDescribe = "这是系统创建默认的相册。";
                pc.picClsPicCnt = 0;
                pc.picClsCreateTime = DateTime.Now;
                db.PictureClass.Add(pc);

                db.SaveChanges();
                return RedirectToAction("Index", "login");
            }

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
