using System;
using System.Runtime.Serialization;

namespace COES.MVC.Extranet.Areas.PMPO.Models
{
    [DataContract]
    public class ObraDetailModel
    {
        [DataMember(Name = "barId")]
        public int? BarId { get; set; }

        [DataMember(Name = "teamId")]
        public int? TeamId { get; set; }

        [DataMember(Name = "groupId")]
        public int? GroupId { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}