using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSkills.Controllers
{
    public class ScratchPadController : Controller
    {
        // GET: ScratchPad
        public ActionResult Index()
        {
            return View();
        }
    }
}