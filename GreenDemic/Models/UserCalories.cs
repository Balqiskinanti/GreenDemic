using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class UserCalories
    {
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Weight (Kg)")]
        [Required]
        public int Weight { get; set; }

        [Display(Name = "Height (Cm)")]
        [Required]
        public int Height { get; set; }

        [Display(Name = "Birthday")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Exercise Type")]
        [Required]
        public string ExerciseType { get; set; }
    }
}
