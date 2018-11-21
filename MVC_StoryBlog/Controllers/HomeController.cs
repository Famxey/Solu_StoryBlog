using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StoryBlog.Models;

namespace MVC_StoryBlog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private StoryBlog_DBEntities db = new StoryBlog_DBEntities();

        //随机照片 +相册浏览
        public ActionResult Index()
        {
            var q = (from i in db.PictureInfo
                     join c in db.PictureClass on i.PicClsID equals c.ID
                     orderby i.picCreateTime descending
                     where c.picClsAuthority == 1
                     select new PicHelper
                     {
                         ID = i.ID,
                         picDescribe = i.picDescribe,
                         Name = i.Name,
                         ImgFile = i.ImgFile,
                         picClsTitle = c.picClsTitle,
                         PicClsID = i.PicClsID,
                         uAccount = i.uAccount,
                     }).ToList();

            if (q.Count > 16)
            {
                List<PicHelper> list = new List<PicHelper>();
                int[] arr = new int[16];

                Random rand = new Random();

                bool[] flag = new bool[q.Count];
                int num = 0;
                for (int i = 0; i < 16; i++)
                {
                    do
                    {
                        // 如果产生的数相同继续循环
                        num = rand.Next(q.Count);
                    } while (flag[num]);
                    flag[num] = true;
                    arr[i] = num;
                }

                for (int j = 0; j < 16; j++)
                {
                    list.Add(q[arr[j]]);
                }
                q = list;
            }

            return View(q.ToList());
        }

        //联系我们的方式
        public ActionResult Contact()
        {
            return View();
        }

        //定义全局变量 接受文章的编号
        private static string artider;

        //浏览文章     
        public ActionResult SelectArt(string id)
        {
            string account;
            string aid = id;
            try
            {

                //account = System.Web.HttpContext.Current.Session["Accout"].ToString();
                account = Server.HtmlEncode(Request.Cookies["Accout"].Value);
                if (account != null)
                {
                    var user = db.UserInfo.Where(u => u.Account == account);
                    ViewBag.picture = user.FirstOrDefault().Picture;
                }
            }
            catch (Exception)
            {

                account = null;
            }
            finally
            {
                artider = id;
                ArticleInfo arti = db.ArticleInfo.Where(a => a.artNo == aid).FirstOrDefault();
                arti.artHot = arti.artHot + 1;
                db.SaveChanges();


                var art = (from i in db.ArticleInfo
                           join u in db.UserInfo
                           on i.uAccount equals u.Account
                           where i.artNo == id
                           select new ArtHelper
                           {
                               ID = i.ID,
                               Title = i.Title,
                               artNo = i.artNo,
                               artCreateTime = i.artCreateTime,
                               NickName = u.NickName,
                               uAccount = i.uAccount,
                               artContent = i.artContent,
                               artHot = i.artHot,
                               artComCnt = i.artComCnt,
                           }
               ).FirstOrDefault();

                ViewBag.NickName = art.NickName;
                ViewBag.uAccount = art.uAccount;
                ViewBag.Title = art.Title;
                ViewBag.artContent = art.artContent;
                ViewBag.artHot = art.artHot;
                ViewBag.artComCnt = art.artComCnt;
                ViewBag.artCreateTime = art.artCreateTime;

            }

            return View();
        }

        //查看文章评论
        public ActionResult SelectArtCM()
        {
            var art = db.ArticleInfo.Where(u => u.artNo == artider);
            ViewBag.ID = art.FirstOrDefault().ID;


            var artcm = from i in db.ArtComment
                        join u in db.UserInfo on i.uAccount equals u.Account
                        orderby i.artCmCreateTime descending
                        where i.artID == art.FirstOrDefault().ID
                        select new artCMhelper
                        {
                            artCmContent = i.artCmContent,
                            NickName = u.NickName,
                            uAccount = u.Account,
                            artCmCreateTime = i.artCmCreateTime
                        };

            return View(artcm.ToList());
        }

        //随机文章浏览8篇
        public ActionResult rendomArticle()
        {
            var q = (from i in db.ArticleInfo
                     orderby i.artCreateTime descending
                     where i.artAuthority == 1
                     select new ArtHelper { ID = i.ID, Title = i.Title, artDigest = i.artDigest, artNo = i.artNo }
               ).ToList();

            if (q.Count > 8)
            {
                List<ArtHelper> list = new List<ArtHelper>();
                int[] arr = new int[10];

                Random rand = new Random();

                bool[] flag = new bool[q.Count];
                int num = 0;
                for (int i = 0; i < 8; i++)
                {
                    do
                    {
                        // 如果产生的数相同继续循环
                        num = rand.Next(q.Count);
                    } while (flag[num]);
                    flag[num] = true;
                    arr[i] = num;
                }

                for (int j = 0; j < 8; j++)
                {
                    list.Add(q[arr[j]]);
                }
                q = list;
            }

            return View(q.ToList());
        }

        //热门文章排行前十榜
        public ActionResult topArticle()
        {
            var q = (from i in db.ArticleInfo
                     orderby i.artHot descending
                     where i.artAuthority == 1
                     select new ArtHelper { artNo = i.artNo, Title = i.Title }
                ).Take(10);
            return View(q.ToList());
        }

        //广告页面
        public ActionResult Banner()
        {
            var adc = (from a in db.AD_Columns
                       orderby a.adCreateTime descending
                       select a).Take(5);

            return View(adc.ToList());
        }

        //释放资源
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
