﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSkills.Controllers
{
    public class KnowledgeHubController : Controller
    {
        [Authorize]
        // GET: KnowledgeHub
        public ActionResult Index()
        {
            ViewBag.Title = "Knowledge Hub";
            return View();
        }
    }
}