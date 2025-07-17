using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_PROPCATALOGODATA
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprPropCatalogoDataDTO : EntityBase
    {
        [DataMember]
        public int Eqcatdcodi { get; set; }
        [DataMember]
        public int? Eqcatccodi { get; set; }
        [DataMember]
        public string Eqcatdabrev { get; set; }
        [DataMember]
        public string Eqcatddescripcion { get; set; }
        [DataMember]
        public int Eqcatdorden { get; set; }
        [DataMember]
        public double Eqcatdvalor { get; set; }
        [DataMember]
        public string Eqcatdnota { get; set; }
        [DataMember]
        public string Eqcatdestregistro { get; set; }
        [DataMember]
        public string Eqcatdusucreacion { get; set; }
        [DataMember]
        public string Eqcatdfeccreacion { get; set; }
        [DataMember]
        public string Eqcatdusumodificacion { get; set; }
        [DataMember]
        public string Eqcatdfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
