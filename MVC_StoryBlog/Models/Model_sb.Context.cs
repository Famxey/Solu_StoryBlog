﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StoryBlog_DBEntities : DbContext
    {
        public StoryBlog_DBEntities()
            : base("name=StoryBlog_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AD_Columns> AD_Columns { get; set; }
        public DbSet<Administration> Administration { get; set; }
        public DbSet<ArtComment> ArtComment { get; set; }
        public DbSet<ArticleClass> ArticleClass { get; set; }
        public DbSet<ArticleInfo> ArticleInfo { get; set; }
        public DbSet<AttentionInfo> AttentionInfo { get; set; }
        public DbSet<PicComment> PicComment { get; set; }
        public DbSet<PictureClass> PictureClass { get; set; }
        public DbSet<PictureInfo> PictureInfo { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}