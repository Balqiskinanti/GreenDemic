using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class Person
    {
        [Display(Name = "User ID")]
        [Required]
        public int UserID { get; set; }

        [Display(Name = "Name")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        [Required]
        public string PersonName { get; set; }

        [Display(Name = "Maximum Calories")]
        public int MaxCal { get; set; }

        [Display(Name = "Account ID")]
        [Required]
        public int AccID { get; set; }
    }
}
