using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XSkills.Models
{
    [MetadataType(typeof(PostsModel))]
    public partial class Post {
    }
    public class PostsModel
    {
        public int Pk_id { get; set; }
        
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please select Section Type")]
        public string Section_Type { get; set; }

        [Required(ErrorMessage = "Please enter something in message box")]
        public string Message { get; set; }
        public string Attachment { get; set; }
        public string Attachment_URL { get; set; }
        [Required(ErrorMessage = "Please select Skills")]
        public string Skills { get; set; }

        public HttpPostedFileBase attachedaudio { get; set; }
        public HttpPostedFileBase attachedvideos { get; set; }
        public HttpPostedFileBase attachedfile { get; set; }
        public HttpPostedFileBase attachedimages { get; set; }
    }

}