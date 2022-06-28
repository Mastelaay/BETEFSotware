using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
    public class CartModel
    {
        [Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }


    }
}
