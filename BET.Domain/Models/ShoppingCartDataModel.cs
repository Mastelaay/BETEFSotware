using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
   public class ShoppingCartDataModel
    {
        public int TempOrderID { get; set; }
        public int PID { get; set; }
        public string PName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
