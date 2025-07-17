using System;
using System.Runtime.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    [DataContract]
    public class ObraListItemModel
    {
        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "idDet")]
        public int IdDet { get; set; }

        [DataMember(Name = "obraTypeId")]

        public int ObraTypeId { get; set; }

        [DataMember(Name = "obraDescripcion")]

        public string ObraDescripcion { get; set; }

        [DataMember(Name = "equinomb")]
        public string Equinomb { get; set; }

        [DataMember(Name = "obraTypeName")]
        public string ObraTypeName { get; set; }

        [DataMember(Name = "plannedDate")]
        public string PlannedDate { get; set; }

        [DataMember(Name = "notes")]
        public string Notes { get; set; }

        [DataMember(Name = "barranomb")]
        public string Barranomb { get; set; }

        [DataMember(Name = "gruponomb")]
        public string Gruponomb { get; set; }
        
        [DataMember(Name = "obraEnFormato")]
        public string ObraEnFormato { get; set; }

    }
}