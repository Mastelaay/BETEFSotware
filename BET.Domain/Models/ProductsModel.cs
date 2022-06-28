using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
   
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int SID { get; set; }
        public int ROL { get; set; }
        public string ImageUrl { get; set; }
    }
}
