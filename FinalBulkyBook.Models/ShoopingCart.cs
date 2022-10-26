using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.Models
{
    public class ShoopingCart
    {
        public Product Product { get; set; }
        [Range(0, 1000, ErrorMessage = "Checkout item must be between 0-1000 item.")]
        public int Count { get; set; }
    }
}
