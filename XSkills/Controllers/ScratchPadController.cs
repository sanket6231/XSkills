using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XSkills.Models;

namespace XSkills.Controllers
{
    [Authorize]
    public class ScratchPadController : Controller
    {
        XSkillsEntities1 context = new XSkillsEntities1();
        ApplicationDbContext dbContext = new ApplicationDbContext();
        

        // GET: ScratchPad
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CommentPartial()
        {
            string userid = User.Identity.Name;
            string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;
            MessagesRepository _messageRepository = new MessagesRepository();
            //return PartialView("_MessagesList", _messageRepository.GetAllMessages(0, username));
            var comments = _messageRepository.GetAllMessages(0, username).OrderByDescending(x => x.CommentDate).ToList();
            //var comments = context.sp_GetMessages(username: username, commentID: null).OrderByDescending(x => x.CommentDate).ToList();
            return PartialView("_GetMessagesPartial", comments);
        }


        public JsonResult addNewComment(commentDTO comment)
        {
            try
            {
                string userid = User.Identity.Name;
                string username = dbContext.Users.Where(x => x.Email == userid).FirstOrDefault().Name;
                
                var _comment = new ScratchPad()
                {
                    ParentId = comment.parentId,
                    MesgFrom = "Reply",
                    MesgTo = username,
                    CommentText = comment.commentText,
                    Username = username,
                    CommentDate = DateTime.Now
                };

                context.ScratchPads.Add(_comment);
                context.SaveChanges();
                var model = context.sp_GetMessages(username: username, commentID: _comment.CommentID)
                        .Select(x => new commentViewModel
                        {
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
    }
}