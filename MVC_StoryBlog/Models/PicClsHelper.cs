using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StoryBlog.Models
{
    public class PicClsHelper
    {
        public int ID { get; set; }
        public string picClsTitle { get; set; }
        public string CoverFile { get; set; }
        public string uAccount { get; set; }
        public int picClsAuthority { get; set; }
        public string picClsDescribe { get; set; }
        public int picClsPicCnt { get; set; }
        public System.DateTime picClsCreateTime { get; set; }
    }
}