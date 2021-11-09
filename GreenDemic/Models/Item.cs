using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Greendemic.Models
{
    public class Item
    {
        [Required]
        [StringLength(10)]
        public int ItemID { get; set; }
        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [StringLength(10)]
        public int Cal { get; set; }
    }
}