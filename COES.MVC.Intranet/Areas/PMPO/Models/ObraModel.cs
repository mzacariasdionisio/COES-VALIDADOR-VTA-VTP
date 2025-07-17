using System;
using System.Runtime.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    [DataContract]
    public class ObraModel
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "companyId")]
        public int CompanyId { get; set; }

        [DataMember(Name = "obraTypeId")]
        public int ObraTypeId { get; set; }

        [DataMember(Name = "plannedDate")]
        public string PlannedDate { get; set; }

        [DataMember(Name = "notes")]
        public string Notes { get; set; }

        [DataMember(Name = "details")]
        public ObraDetailModel[] Details { get; set; }

        [DataMember(Name = "formato")]
        public int Formato { get; set; }

        [DataMember(Name = "ObraFlagFormat")]
        public int ObraFlagFormat { get; set; }
    }
}