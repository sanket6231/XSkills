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
        XSkillsEntities1 context = new XSkillsEntities1();
        ApplicationDbContext dbContext = new ApplicationDbContext();

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
            return PartialView("ProfileDetails_popup", context.User_Profile.Find(name));
        }

        [HttpPost]
        public ActionResult AddQuestion(string Name,string comment)
        {
            string userid = User.Identity.Name;
            string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;

            var _scratchpad = new ScratchPad()
            {
                ParentId = 0,
                CommentText = comment,
                Username = username,
                MesgFrom = username,
                MesgTo = Name,
                CommentDate = DateTime.Now
            };

            context.ScratchPads.Add(_scratchpad);
            context.SaveChanges();

            return RedirectToAction("Index");
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