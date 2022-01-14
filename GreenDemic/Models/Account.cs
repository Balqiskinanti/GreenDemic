using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class Account
    {
        [Display(Name = "Account ID")]
        [Required]
        public int AccID { get; set; }

        [Display(Name = "Account Name")]
        [StringLength(50, ErrorMessage = "Account Name cannot exceed 50 characters")]
        [Required]
        public string AccName { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email address cannot exceed 50 characters")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
        [Required]
        public string Pass_word { get; set; }

        public string Bio { get; set; }

        [Display(Name = "Mute Notifications")]
        [Required]
        public bool IsMuted { get; set; }

        [Display(Name = "Health Points")]
        [Required]
        public int Health { get; set; }

        [Display(Name = "Quiz Points")]
        [Required]
        public int QuizPoints { get; set; }
    }
}
