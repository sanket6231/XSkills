using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XSkills.Models
{
    public class EditProfileModel : User_Profile
    {
        //public string ImgUrl { get; set; }
        public HttpPostedFileBase postedFile { get; set; }
        //public string Wave { get; set; }
        //public string Skills { get; set; }
        //public string Certifications { get; set; }
        //public string Trainings { get; set; }
    }
}