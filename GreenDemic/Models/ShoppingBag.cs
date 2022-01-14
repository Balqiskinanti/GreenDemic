using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class ShoppingBag
    {
        [Display(Name = "Shopping Bag ID")]
        [Required]
        public int ShoppingBagID { get; set; }

        [Display(Name = "Account ID")]
        [Required]
        public int AccID { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Shopping Bag Name")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [Required(ErrorMessage = "Please enter a name!")]
        public string BagName { get; set; }

        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string BagDescription { get; set; }

        public String Location { get; set; }

        [Display(Name = "Save as Preset")]
        [Required]
        public bool IsPreset { get; set; }

        [Display(Name = "Used a Preset")]
        [Required]
        public int UsedPreset { get; set; }
    }
}
