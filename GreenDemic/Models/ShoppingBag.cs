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

        [Display(Name = "Created At")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Bag Name")]
        [StringLength(50, ErrorMessage = "Bag Name cannot exceed 50 characters")]
        [Required]
        public int BagName { get; set; }

        [Display(Name = "Bag Description")]
        [StringLength(255, ErrorMessage = "Bag Description cannot exceed 50 characters")]
        [Required]
        public int BagDescription { get; set; }

        [Display(Name = "Account ID")]
        [Required]
        public int AccID { get; set; }
    }
}
