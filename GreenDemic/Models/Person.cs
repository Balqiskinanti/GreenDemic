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

        [Display(Name = "Family Member Name")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Maximum Calories / day")]
        public int? MaxCal { get; set; }

        [Display(Name = "Height (Cm)")]
        [Required]
        public int Height { get; set; }

        [Display(Name = "Weight (Kg)")]
        [Required]
        public int Weight { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        public int Gender { get; set; }

        [Display(Name = "Exercise Type")]
        [Required]
        public int ExType { get; set; }

        [Display(Name = "Account ID")]
        [Required]
        public int AccID { get; set; }

        public int GetAge()
        {
            // Calculate age based on birth year
            int age;
            age = DateTime.Now.Year - Birthday.Year;
            if (DateTime.Now.DayOfYear < Birthday.DayOfYear)
            {
                age--;
            }
            return age;
        }

        // Return AMR 
        // AMR represents the number of calories to consume to stay at your current weight
        private double CalculateAMR(double BMR, int exerciseType)
        {
            double AMR = 0;
            if (exerciseType == 0) // 0 = Sedentary
            {
                AMR = BMR * 1.2;
            }
            else if (exerciseType == 1) // 1 = Lightly Active
            {
                AMR = BMR * 1.375;
            }
            else if (exerciseType == 2) // 2 = Moderately Active
            {
                AMR = BMR * 1.55;
            }
            else if (exerciseType == 3) // 3 = Active
            {
                AMR = BMR * 1.725;
            }
            else if (exerciseType == 4) // 4 = Very Active
            {
                AMR = BMR * 1.9;
            }
            return AMR;
        }

        public int CalculateBMR()
        {
            // Calculate basal metabolic rate based on
            // Gender, weight, height, and age
            int age = GetAge();
            double BMR = 0;

            if (Gender == 0) // 0 = female
            {
                BMR = 655.1 + (9.563 * Weight) + (1.850 * Height) - (4.676 * age);
            }
            else if (Gender == 1)
            {
                BMR = 66.47 + (13.75 * Weight) + (5.003 * Height) - (6.755 * age);
            }

            // Calculate active metabolic rate based on BMR and current exercise/ activity level
            int bmr = (int)CalculateAMR(BMR, ExType);

            return bmr;
        }
    }
}
