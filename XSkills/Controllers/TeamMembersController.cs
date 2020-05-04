using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XSkills.Models;

namespace XSkills.Controllers
{
    [Authorize]
    public class TeamMembersController : Controller
    {
        // GET: TeamMembers
        public ActionResult Index()
        {
            TeamMembersModel model = PopulateModel(null);
            return View(model);

        }

        [HttpPost]
        public ActionResult Index(string skills)
        {
            TeamMembersModel model = PopulateModel(skills);
            return View(model);
        }

        
        public ActionResult Details(string name)
        {
            XSkillsEntities1 entities = new XSkillsEntities1();
            return PartialView("ProfileDetails_popup", entities.User_Profile.Find(name));
        }

        private static TeamMembersModel PopulateModel(string skill)
        {
            using (XSkillsEntities1 entities = new XSkillsEntities1())
            {
                TeamMembersModel model = new TeamMembersModel()
                {
                    Members = (from c in entities.User_Profile
                                 where c.Skills.Contains(skill) || string.IsNullOrEmpty(skill)
                                 select c).ToList(),
                    Skills = (from c in entities.User_Profile
                                 select new SelectListItem { Text = c.Skills, Value = c.Skills }).Distinct().ToList()
                };

                return model;
            }
        }
    }
}