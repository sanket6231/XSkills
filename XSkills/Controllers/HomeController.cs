using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using XSkills.Models;

namespace XSkills.Controllers
{
    public class HomeController : Controller
    {

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
        public ActionResult LoginHome()
        {
            string user = User.Identity.GetUserName();
            //var model = _userManager.FindByEmail(user);
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ViewBag.username = dbContext.Users.Where(x => x.Email == user).FirstOrDefault().Name;
            return View();
        }
    }
}