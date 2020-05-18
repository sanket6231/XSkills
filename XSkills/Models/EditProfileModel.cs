using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSkills.Models
{
    public class EditProfileModel : User_Profile
    {
        [Display(Name = "Aspiration Skills")]
        public List<SelectListItem> AspirationSkills { get; set; }
        public string[] AspirationSkillsIds { get; set; }

    }
}