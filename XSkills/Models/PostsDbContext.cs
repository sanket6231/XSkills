using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace XSkills.Models
{
    public class PostsDbContext : DbContext
    {
        public DbSet<PostsModel> Posts { get; set; }
    }
}