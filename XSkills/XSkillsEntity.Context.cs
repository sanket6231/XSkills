﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XSkills
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class XSkillsEntities1 : DbContext
    {
        public XSkillsEntities1()
            : base("name=XSkillsEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User_Profile> User_Profile { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<ParaMaster> ParaMasters { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
    
        public virtual ObjectResult<sp_Filter_Posts_Result> sp_Filter_Posts(string postedBy, string sectionType, string skills)
        {
            var postedByParameter = postedBy != null ?
                new ObjectParameter("PostedBy", postedBy) :
                new ObjectParameter("PostedBy", typeof(string));
    
            var sectionTypeParameter = sectionType != null ?
                new ObjectParameter("SectionType", sectionType) :
                new ObjectParameter("SectionType", typeof(string));
    
            var skillsParameter = skills != null ?
                new ObjectParameter("Skills", skills) :
                new ObjectParameter("Skills", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_Filter_Posts_Result>("sp_Filter_Posts", postedByParameter, sectionTypeParameter, skillsParameter);
        }
    
        public virtual ObjectResult<GetComments_Result> GetComments(Nullable<int> postID, Nullable<int> commentID)
        {
            var postIDParameter = postID.HasValue ?
                new ObjectParameter("PostID", postID) :
                new ObjectParameter("PostID", typeof(int));
    
            var commentIDParameter = commentID.HasValue ?
                new ObjectParameter("CommentID", commentID) :
                new ObjectParameter("CommentID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetComments_Result>("GetComments", postIDParameter, commentIDParameter);
        }
    }
}
