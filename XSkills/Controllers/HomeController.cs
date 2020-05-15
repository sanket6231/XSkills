using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using XSkills.Hubs;
using XSkills.Models;

namespace XSkills.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        XSkillsEntities1 xSkills = new XSkillsEntities1();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult LoginHome(string pagename)
        {
            string user = User.Identity.GetUserName();
            //var model = _userManager.FindByEmail(user);
            
            string name = dbContext.Users.Where(x => x.Email == user).FirstOrDefault().Name;
            User_Profile model = xSkills.User_Profile.Where(x => x.Name == name).FirstOrDefault();

            ViewBag.username = name;
            ViewBag.ImageUrl = model.ImgUrl;
            ViewBag.page = pagename;
            return PartialView("Sidebar");
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            string user = User.Identity.GetUserName();
            string name = dbContext.Users.Where(x => x.Email == user).FirstOrDefault().Name;
            
            User_Profile model = xSkills.User_Profile.Where(x => x.Name == name).FirstOrDefault();
            //ViewBag.ImageUrl = model.ImgUrl;
            //ViewBag.username = name;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditProfile(User_Profile model, HttpPostedFileBase postedFile)
        {
            //string username = (string)TempData["UserName"];
            if (postedFile != null)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string path = Server.MapPath("~/UploadedFiles/Images/profiles/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + fileName);
                ViewBag.ImageUrl = "~/UploadedFiles/Images/profiles/" + fileName;
            }

            using (XSkillsEntities1 entities = new XSkillsEntities1())
            {
                User_Profile xmodel = entities.User_Profile.Where(x => x.Name == model.Name).FirstOrDefault();
                //xmodel.Name = model.Name;
                xmodel.Skills = model.Skills;
                xmodel.Wave = model.Wave;
                xmodel.Trainings = model.Trainings;
                xmodel.Certifications = model.Certifications;
                xmodel.ImgUrl = ViewBag.ImageUrl == null ? model.ImgUrl : ViewBag.ImageUrl;
                model.ImgUrl = ViewBag.ImageUrl == null ? model.ImgUrl : ViewBag.ImageUrl;
                entities.SaveChanges();
            }

            return View(model);
        }

        [Authorize]
        public ActionResult RateUs()
        {
            ViewBag.Title = "Rate Us";
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult RateUs(RatingModel rating)
        {
            if (ModelState.IsValid)
            {
                using (XSkillsEntities1 entities = new XSkillsEntities1())
                {
                    Rating rate = new Rating()
                    {
                        Userid = User.Identity.Name,
                        Rating1 = rating.RatingNo,
                        Feedback = rating.Feedback
                    };
                    entities.Ratings.Add(rate);
                    entities.SaveChanges();
                }
                
            }
            return View();
        }

        [Authorize]
        public JsonResult GetNotificationContacts()
        {
            string user = User.Identity.GetUserName();
            string name = dbContext.Users.Where(x => x.Email == user).FirstOrDefault().Name;

            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetNotifications(Convert.ToDateTime(Session["LastUpdated"]), name);
            //update session here for get only new added contacts (notification)
            Session["LastUpdated"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}