﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StoryBlog.Models
{
    public class ArtHelper
    {
        public int ID { get; set; }
        public string artNo { get; set; }
        public string Title { get; set; }
        public string artContent { get; set; }
        public System.DateTime artCreateTime { get; set; }
        public int artClsID { get; set; }
        public string uAccount { get; set; }
        public int artAuthority { get; set; }
        public Nullable<int> artHot { get; set; }
        public int artComCnt { get; set; }
        public string artDigest { get; set; }
        public string artClsTitle { get; set; }
        public string NickName { get; set; }

        public string artCreateTimeA
        {
            get { return artCreateTime.ToString("yyyy/MM/dd/HH:mm:ss"); }
        }

    }
}