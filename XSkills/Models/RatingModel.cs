using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XSkills.Models
{
    public class RatingModel
    {
        [Required]
        [Range(1,5, ErrorMessage ="Rating should be between {0} and {1}")]
        public int RatingNo { get; set; }

        [Required]
        [StringLength(maximumLength: 200,ErrorMessage ="Message cannot exceed {0} characters")]
        public string Feedback { get; set; }
    }
}