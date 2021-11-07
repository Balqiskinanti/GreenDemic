using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Greendemic.Models
{
    public class ShoppingBagItem
    {
        [Required]
        [StringLength(10)]
        public int ShoppingBagID { get; set; }
        [Required]
        [StringLength(10)]
        public int ItemID { get; set; }
        [Required]
        [StringLength(10)]
        public int Qty { get; set; }
    }
}
