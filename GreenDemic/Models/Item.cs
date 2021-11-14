using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Greendemic.Models
{
    public class Item
    {
        [Display(Name = "Item ID")]
        [Required]
        public int ItemID { get; set; }

        [Display(Name = "Item Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Item Name cannot exceed 50 characters")]
        public string ItemName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; }

        [Display(Name = "Calories")]
        [Required]
        public int Cal { get; set; }
    }
}