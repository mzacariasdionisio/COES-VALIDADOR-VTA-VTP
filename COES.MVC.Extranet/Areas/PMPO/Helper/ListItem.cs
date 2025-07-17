using System;
using System.Runtime.Serialization;

namespace COES.MVC.Extranet.Areas.PMPO.Helper
{
    [DataContract(Name = "items")]
    public class ListItem
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}