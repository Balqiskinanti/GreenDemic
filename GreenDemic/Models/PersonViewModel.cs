using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class PersonViewModel
    {
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Display(Name = "Family Member Name")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string UserName { get; set; }

        [Display(Name = "Maximum Calories / day")]
        public int MaxCal { get; set; }

        [Display(Name = "Maximum Calories / day")]
        public int DerivedMaxCal { get; set; }

        public int Age { get; set; }
    }
}
