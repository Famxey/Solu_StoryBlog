//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_StoryBlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        public string Account { get; set; }
        public int ID { get; set; }
        public string NickName { get; set; }
        public string PassWord { get; set; }
        public string Picture { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public string Describe { get; set; }
        public string Introduce { get; set; }
    }
}
