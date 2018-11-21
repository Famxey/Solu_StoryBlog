using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_StoryBlog.Models
{
    public class artCMhelper
    {
        public int ID { get; set; }
        public string artCmContent { get; set; }
        public string uAccount { get; set; }
        public string NickName { get; set; }
        public int artID { get; set; }
        public System.DateTime artCmCreateTime { get; set; }
    }
}