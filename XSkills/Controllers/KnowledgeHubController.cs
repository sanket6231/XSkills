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
            
            var model = context.sp_FilterPosts("All","All","All").OrderByDescending(x => x.CreatedDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PostsViewModel filters)
        {
            //List<Post>
            var model = context.sp_FilterPosts(filters.PostedBy,filters.Section_Type,filters.Skills).OrderByDescending(x => x.CreatedDate).ToList();
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
                string isAttached = "N", attachment_url = "";

                if (posts.attachedfile != null)
                {
                    string fileName = Path.GetFileName(posts.attachedfile.FileName);
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    posts.attachedfile.SaveAs(path + fileName);

                    isAttached = "Y";
                    attachment_url = path + fileName;
                }
                if (posts.attachedaudio != null)
                {
                    string fileName = Path.GetFileName(posts.attachedaudio.FileName);
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    posts.attachedaudio.SaveAs(path + fileName);

                    isAttached = "Y";
                    attachment_url = path + fileName;
                }
                if (posts.attachedimages != null)
                {
                    string fileName = Path.GetFileName(posts.attachedimages.FileName);
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    posts.attachedimages.SaveAs(path + fileName);

                    isAttached = "Y";
                    attachment_url = path + fileName;
                }
                if (posts.attachedimages != null)
                {
                    string fileName = Path.GetFileName(posts.attachedimages.FileName);
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    posts.attachedimages.SaveAs(path + fileName);

                    isAttached = "Y";
                    attachment_url = path + fileName;
                }

                using (XSkillsEntities1 entities = new XSkillsEntities1())
                {
                    Post post = new Post()
                    {
                        UserId = userid,
                        Section_Type = posts.Section_Type,
                        Message = posts.Message,
                        Attachment = isAttached,
                        Attachment_URL = attachment_url,
                        Skills = posts.Skills,
                        CreatedDate = DateTime.Now,
                        UserName = username
                    };
                    entities.Posts.Add(post);
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
                //return Json(new { success = true, url = Url.Action("Index", "KnowledgeHub") });
            }
            //return RedirectToAction("Index");
            ViewBag.Skills = Getitems("Skills");
            ViewBag.PersonalSkills = Getitems("PersonalSkills");
            return Json(new { success = true, error = "Error"});
        }

        public PartialViewResult CommentPartial(int postid)
        {
            //XSkillsEntities1 context = new XSkillsEntities1();
            var comments = context.Comments.Where(x => x.PostID == postid).OrderBy(x => x.CommentDate);
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
                var model = context.Comments.Where(x => x.CommentID == _comment.CommentID)
                        .Select(x => new commentViewModel
                        {
                            PostId = x.PostID,
                            commentId = x.CommentID,
                            commentText = x.CommentText,
                            parentId = x.ParentId,
                            commentDate = x.CommentDate,
                            username = x.Username

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
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var users = dbContext.Users.Select(u => u.Name).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

    }
}