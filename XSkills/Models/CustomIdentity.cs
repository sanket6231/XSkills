using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XSkills.Models
{
    public class CustomIdentity : IdentityUser
    {
        public string Name { get; set; }
    }
}