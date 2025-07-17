using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprProyectoActEqpDTO : EntityBase
    {
        [DataMember]
        public int Epproycodi { get; set; }
        [DataMember]
        public int? Epproysgcodi { get; set; }
        [DataMember]
        public string Eppproyflgtieneeo { get; set; }
        [DataMember]
        public int? Emprcodi { get; set; }
        [DataMember]
        public string Epproynemotecnico { get; set; }
        [DataMember]
        public string Epproynomb { get; set; }
        [DataMember]
        public string Epproyfecregistro { get; set; }
        [DataMember]
        public string Epproydescripcion { get; set; }
        [DataMember]
        public string Epproyestregistro { get; set; }
        [DataMember]
        public string Epproyusucreacion { get; set; }
        [DataMember]
        public string Epproyfeccreacion { get; set; }
        [DataMember]
        public string Eppproyusumodificacion { get; set; }
        [DataMember]
        public string Epproyfecmodificacion { get; set; }
        public string Area { get; set; }

        public string Emprnomb { get; set; }

        public int NroEquipo { get; set; }
        public int NroPropiedades { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public DateTime Epproyfecregistrodate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
