using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BET.Domain.Models
{
    [DataContract]
    public class JsonResponseModel
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string ResultMessage { get; set; }
        [DataMember]
        public int ResourceId { get; set; }
        [DataMember]
        public string Total { get; set; }
    }
    [DataContract]
    public class JsonResponseModel<T>
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string ResultMessage { get; set; }
        [DataMember]
        public T Result { get; set; }
    }
}