using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Greendemic.Models
{
    public class ShoppingBagItem
    {
        [Display(Name = "Shopping Bag ID")]
        [Required]
        public int ShoppingBagID { get; set; }

        [Display(Name = "Item ID")]
        [Required]
        public int ItemID { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public int Qty { get; set; }
    }
}