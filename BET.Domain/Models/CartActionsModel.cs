using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
  public  class CartActionsModel
    {

        public int Type { get; set; }
        public int ProductId { get; set; }
        public int LoggedInUser { get; set; }
     
    }
}
