using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSkills.Models
{
    public class TeamMembersModel
    {
        public List<SelectListItem> Skills { get; set; }
        public List<User_Profile> Members { get; set; }
    }
}