using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XSkills.Models
{
    public class ScratchPadModel
    {
        public int CommentID { get; set; }
        public string Username { get; set; }
        public string MesgFrom { get; set; }
        public string MesgTo { get; set; }
        public string CommentText { get; set; }
        public System.DateTime CommentDate { get; set; }
        public int ParentId { get; set; }
        public string ImgUrl { get; set; }
    }
}