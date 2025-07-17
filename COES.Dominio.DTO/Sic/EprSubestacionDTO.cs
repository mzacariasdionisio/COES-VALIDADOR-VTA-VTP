using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_SUBESTACION
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprSubestacionDTO : EntityBase
    {
        [DataMember]
        public int Epsubecodi { get; set; }
        [DataMember]
        public int? Areacodi { get; set; }
        [DataMember]
        public string Areanomb { get; set; }
        [DataMember]
        public int? Epproycodi { get; set; }
        [DataMember]
        public string Epproynomb { get; set; }
        [DataMember]
        public string Epsubemotivo { get; set; }
        [DataMember]
        public string Epsubefecha { get; set; }
        [DataMember]
        public DateTime Epsubefechadate { get; set; }
        [DataMember]
        public string Epsubememoriacalculo { get; set; }
        [DataMember]
        public string Epsubeestregistro { get; set; }
        [DataMember]
        public string Epsubeusucreacion { get; set; }
        [DataMember]
        public string Epsubefeccreacion { get; set; }
        [DataMember]
        public string Epsubeusumodificacion { get; set; }
        [DataMember]
        public string Epsubefecmodificacion { get; set; }
        [DataMember]
        public string EpsubememoriacalculoText { get; set; }
        [DataMember]
        public string UltMemoriaCalculo { get; set; }
        


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
