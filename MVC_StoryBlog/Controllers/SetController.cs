using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;
using System.IO;

namespace MVC_StoryBlog.Controllers
{
    public class SetController : Controller
    {
        //
        // GET: /Set/


        //定义全局变量 接受Session 传值
        //private static string account = System.Web.HttpContext.Current.Session["Accout"].ToString();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Frame()
        {
            return View();
        }

        public ActionResult setLeft()
        {
            return View();
        }

        public ActionResult setRight()
        {
            return View();
        }

        //个人信息
        public ActionResult UserInfo()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            ViewBag.User = account;
            //string account = Session["User"].ToString();

            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var user = db.UserInfo.Where(u => u.Account == account);
                ViewBag.pic = user.FirstOrDefault().Picture;
                ViewBag.nikename = user.FirstOrDefault().NickName;
                ViewBag.gender = user.FirstOrDefault().Gender;
                ViewBag.describe = user.FirstOrDefault().Describe;
                ViewBag.introduce = user.FirstOrDefault().Introduce;

            }

            return View();

        }

        //上传头像
        public ActionResult UploadPic()
        {
            Select();
            return View();
        }
        [HttpPost]
        public ActionResult UploadPic(HttpPostedFileBase file)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);

            if (file != null)
            {
                FileInfo fi = new FileInfo(file.FileName);
                string hz = fi.Extension;

                string time = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                if (file.ContentLength > 0 && hz == ".jpg" || hz == ".gif" || hz == ".png")
                {
                    file.SaveAs(Server.MapPath("~/UploadPicture/HeadPicture/" + time + hz));
                    using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
                    {
                        var user = db.UserInfo.Where(u => u.Account == account).FirstOrDefault();
                        user.Picture = "/UploadPicture/HeadPicture/" + time + hz;
                        db.SaveChanges();
                    }
                    Select1();
                    Session["pic"] = null;
                    ViewBag.cg = "上传成功!!!";
                }
                else
                {
                    ViewBag.cg1 = "SORRY!!!上传失败!!!请用.jpg/.gif/.png的照片上传。";
                    return View();
                }

            }
            else
            {
                ViewBag.cg1 = "SORRY!!!未选择上传的文件!!!";
                return View();
            }

            return View();
        }


        //修改资料
        public ActionResult UpdateInfo()
        {
            Select1();
            return View();
        }

        [HttpPost]
        public ActionResult UpdateInfo(string nickname, string gender, string describe, string introduce)
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            if (nickname != "")
            {
                if (describe.Length <= 150 && introduce.Length <= 150)
                {
                    using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
                    {
                        var user = db.UserInfo.Where(u => u.Account == account);
                        user.FirstOrDefault().NickName = nickname;
                        user.FirstOrDefault().Gender = gender;
                        user.FirstOrDefault().Introduce = introduce;
                        user.FirstOrDefault().Describe = describe;
                        db.SaveChanges();
                    }
                    Session["User"] = nickname;

                    ViewBag.Info = "您的昵称已修改为" + nickname + "，资料信息亦更新成功！";
                }
                else
                {
                    ViewBag.Info4 = "SORRY!!!自我介绍或个人描述的字数已超过了150字！！！";
                }
            }
            else
            {
                ViewBag.Info4 = "SORRY!!!昵称不能为空！！！";
            }

            return View();
        }

        //修改密码
        public ActionResult UpdatePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePwd(string oldpwd, string pwd, string repwd)
        {

            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var user = db.UserInfo.Where(u => u.Account == account);
                string PassWord = user.FirstOrDefault().PassWord;

                //判断密码是否为空
                if (oldpwd != "" && pwd != "" && repwd != "")
                {

                    //判断原始密码是否正确
                    if (PassWord == oldpwd)
                    {
                        //判断是否包含英文字符
                        char[] ch = new char[pwd.Length];
                        ch = pwd.ToCharArray();
                        bool flag = false;
                        foreach (var item in ch)
                        {
                            if ((item >= 'a' && item < 'z') || (item >= 'A' && item < 'Z'))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            //判断输入新密码的长度
                            if (pwd.Length >= 6 && pwd.Length <= 12)
                            {
                                //判断两次密码是否一致
                                if (pwd == repwd)
                                {
                                    user.FirstOrDefault().PassWord = repwd;

                                    db.SaveChanges();

                                    ViewBag.Info1 = "密码修改成功！";

                                }
                                else
                                {
                                    ViewBag.Info2 = "SORRY!!!两次密码输不一致！";
                                }
                            }
                            else
                            {
                                ViewBag.Info2 = "SORRY!!!密码不符合输入的规格！请输入6到12位的英文字母或数字。";
                            }
                        }

                        else
                        {
                            ViewBag.Info2 = "SORRY!!!密码不符合输入的规格！请输入包含英文字母的6到12位的密码。";
                        }
                    }
                    else
                    {
                        ViewBag.Info2 = "SORRY!!!原密码输入错误！";
                    }
                }
                else
                {
                    ViewBag.Info2 = "SORRY!!!密码不能为空，请输入密码！";

                }
            }

            return View();
        }

        private void Select()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var user = db.UserInfo.Where(u => u.Account == account);
                Session["pic"] = user.FirstOrDefault().Picture;
            }
        }

        private void Select1()
        {
            string account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
            using (StoryBlog_DBEntities db = new StoryBlog_DBEntities())
            {
                var user = db.UserInfo.Where(u => u.Account == account);
                ViewBag.pic1 = user.FirstOrDefault().Picture;
                ViewBag.unick = user.FirstOrDefault().NickName;
            }
        }

    }
}
