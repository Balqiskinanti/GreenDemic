using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Models
{
    public class ItemViewModel
    {
        [Display(Name = "Item ID")]
        [Required]
        public int ItemID { get; set; }

        [Display(Name = "Shopping Bag ID")]
        [Required]
        public int ShoppingBagID { get; set; }

        [Display(Name = "Shopping Bag Name")]
        public string ShoppingBagName { get; set; }

        [Display(Name = "Item Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Item Name cannot exceed 50 characters")]
        public string ItemName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; }

        [Display(Name = "Calories(per item)")]
        [Required]
        public int Cal { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public int Qty { get; set; }

        [Display(Name = "Calorie Sub Total")]
        public int CalSubTotal { get; set; }
    }
}
