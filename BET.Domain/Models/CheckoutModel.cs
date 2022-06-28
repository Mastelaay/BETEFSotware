using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BET.Domain.Models
{
    [DataContract]
  public class CheckoutModel
    {
        [DataMember]
        public List<ProductsModel> Products { get; set; }
        [DataMember]
        public UsersModel User { get; set; }
        [DataMember]
        public string TotalPrice { get; set; }
    }
}
