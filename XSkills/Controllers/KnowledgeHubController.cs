using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XSkills.Models;

namespace XSkills.Controllers
{
    [Authorize]
    public class KnowledgeHubController : Controller
    {
        XSkillsEntities1 context = new XSkillsEntities1();
        ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: KnowledgeHub
        public ActionResult Index()
        {
            ViewBag.Title = "Knowledge Hub";
            string userid = User.Identity.Name;
            string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;
            ViewBag.username = username;
            var model = context.sp_Filter_Posts("All","All","All").OrderByDescending(x => x.CreatedDate).ToList();
            
            var currentTime = dbContext.Users.Where(x => x.Email == User.Identity.Name).First().LastLoginOn;
            Session["LastUpdated"] = currentTime;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PostsViewModel filters)
        {
            //List<Post>
            var model = context.sp_Filter_Posts(filters.PostedBy,filters.Section_Type,filters.Skills).OrderByDescending(x => x.CreatedDate).ToList();
            //foreach (var item in model) {
            //}
            //return RedirectToAction("Index", model);
            return View(model);
        }

        public ActionResult SharePost()
        {
            ViewBag.Skills = Getitems("Skills");
            ViewBag.PersonalSkills = Getitems("PersonalSkills");
            return PartialView("SharePostPartial");
        }

        [HttpPost]
        public ActionResult SharePostSubmit(PostsModel posts)
        {
            string userid = User.Identity.Name;
            string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;

            if (!ModelState.IsValid)
            {
                var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
                var message = "Please fix the following: " + Environment.NewLine;

                foreach (var modelStateError in modelStateErrors)
                {
                    message += modelStateError.ErrorMessage + Environment.NewLine;
                }

                return Json(new { success = false, response = message });
            }
            if (ModelState.IsValid)
            {
                string isAttached = "N";
                List<string> attachment_url = new List<string>();
                List<string> attachement_type = new List<string>();

                if (posts.attachedfile != null)
                {
                    foreach (HttpPostedFileBase file in posts.attachedfile) {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Server.MapPath("~/UploadedFiles/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + fileName);

                        isAttached = "Y";
                        attachment_url.Add("\\UploadedFiles\\" + fileName);
                        attachement_type.Add(file.ContentType);
                    }
                }
                if (posts.attachedaudio != null)
                {
                    foreach (HttpPostedFileBase file in posts.attachedaudio)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Server.MapPath("~/UploadedFiles/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + fileName);

                        isAttached = "Y";
                        attachment_url.Add("\\UploadedFiles\\" + fileName);
                        attachement_type.Add(file.ContentType);
                    }
                }
                if (posts.attachedimages != null)
                {
                    foreach (HttpPostedFileBase file in posts.attachedimages)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Server.MapPath("~/UploadedFiles/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + fileName);

                        isAttached = "Y";
                        attachment_url.Add("\\UploadedFiles\\" + fileName);
                        attachement_type.Add(file.ContentType);
                    }
                }
                if (posts.attachedvideos != null)
                {
                    foreach (HttpPostedFileBase file in posts.attachedvideos)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Server.MapPath("~/UploadedFiles/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + fileName);

                        isAttached = "Y";
                        attachment_url.Add("\\UploadedFiles\\" + fileName);
                        attachement_type.Add(file.ContentType);
                    }
                }

                using (XSkillsEntities1 entities = new XSkillsEntities1())
                {
                    Post post = new Post()
                    {
                        UserId = userid,
                        Section_Type = posts.Section_Type,
                        Message = posts.Message,
                        Attachment = isAttached,
                        Attachment_URL = string.Join(";",attachment_url),
                        Attachment_Type = string.Join(";", attachement_type),
                        Skills = posts.Skills,
                        CreatedDate = DateTime.Now,
                        UserName = username
                    };
                    entities.Posts.Add(post);
                    entities.SaveChanges();
                }
                //return RedirectToAction("Index");
                return Json(new { success = true, url = Url.Action("Index", "KnowledgeHub") });
            }
            //return RedirectToAction("Index");
            ViewBag.Skills = Getitems("Skills");
            ViewBag.PersonalSkills = Getitems("PersonalSkills");
            return Json(new { success = true, error = "Error"});
        }

        public PartialViewResult CommentPartial(int postid)
        {
            //XSkillsEntities1 context = new XSkillsEntities1();
            var comments = context.GetComments(postID: postid, commentID: null).OrderByDescending(x => x.CommentDate).ToList();
            ViewBag.PostId = postid;
            return PartialView("_CommentPartial", comments);
        }

        
        public JsonResult addNewComment(commentDTO comment)
        {
            try
            {
                string userid = User.Identity.Name;
                string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;

                //XSkillsEntities1 context = new XSkillsEntities1();
                var _comment = new Comment()
                {
                    PostID = comment.PostId,
                    ParentId = comment.parentId,
                    CommentText = comment.commentText,
                    Username = username,
                    CommentDate = DateTime.Now
                };

                context.Comments.Add(_comment);
                context.SaveChanges();
                var model = context.GetComments(postID: null,commentID: _comment.CommentID)
                        .Select(x => new commentViewModel
                        {
                            PostId = x.PostID,
                            commentId = x.CommentID,
                            commentText = x.CommentText,
                            parentId = x.ParentId,
                            commentDate = x.CommentDate,
                            username = x.Username,
                            ImgUrl = x.ImgUrl

                        }).FirstOrDefault();
                
                return Json(new { error = false, result = model }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //Handle Error here..
                string exception = ex.Message.ToString();
            }

            return Json(new { error = true }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string fileurl)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(fileurl));
            string fileName = Path.GetFileName(fileurl);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        //------------------------------------------------------------------
        [HttpPost]
        public ActionResult Getitems(string category)
        {
            if (category != null) {
                //XSkillsEntities1 entity = new XSkillsEntities1();
                var items = context.ParaMasters.Where(x => x.Category == category).ToList();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUsers()
        {
            var users = dbContext.Users.Select(u => u.Name).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult displaySuggestions()
        {
            string userid = User.Identity.Name;
            var username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;
            var model = context.User_Profile.Where(x => x.Name == username).FirstOrDefault();
            if (model.Suggestions.ToString() == "Y") {
                string[] skills = model.Aspirational_Skills.Split(',');
                List<string> names = context.User_Profile.Where(x => skills.Contains(x.Skills)).Select(x => x.Name).ToList();
                //var names = context.sp_GetSuggestions(username: username).;
                return Json(new { Suggestions = model.Suggestions.ToString(), Names = string.Join(",", names) }, JsonRequestBehavior.AllowGet);
            }

            
            return Json(new { Suggestions = model.Suggestions.ToString() }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult displaySuggestions(string value)
        {
            string userid = User.Identity.Name;
            var username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;
            if (value == "N") {
                var user_profile = context.User_Profile.Where(x => x.Name == username).FirstOrDefault();
                var _user = new User_Profile() { Name= user_profile.Name,Wave = user_profile.Wave, Suggestions = value };

                using (var newContext = new XSkillsEntities1())
                {
                    newContext.User_Profile.Attach(_user);
                    newContext.Entry(_user).Property(X => X.Suggestions).IsModified = true;
                    newContext.SaveChanges();
                }
            }
            return Json(new { error = false }, JsonRequestBehavior.AllowGet);
        }
    }
}