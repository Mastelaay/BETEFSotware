using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
    [DataContract]
    public class ApiResponseModel
    {
        [DataMember]
        public int ResourceId { get; set; }
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Total { get; set; }
    }

    [DataContract]
    public class ApiItemResponseModel<T> : ApiResponseModel
    {
        [DataMember]
        public T Item { get; set; }
    }
    [DataContract]
    public class ApiItemsResponseModel<T> : ApiResponseModel
    {
        public ApiItemsResponseModel()
        {
            Items = new List<T>();
        }
        [DataMember]
        public List<T> Items { get; set; }
    }
}