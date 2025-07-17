using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_INSUMOS_FACTORK
    /// </summary>
    public partial class IndInsumosFactorKDTO : EntityBase, ICloneable
    {
        public int Insfckcodi { get; set; }
        public int Ipericodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodicentral { get; set; }
        public int Equicodiunidad { get; set; }
        public int Grupocodi { get; set; }
        public int Famcodi { get; set; }
        public decimal? Insfckfrc { get; set; }
        public string Insfckusucreacion { get; set; }
        public DateTime Insfckfeccreacion { get; set; }
        public string Insfckusumodificacion { get; set; }
        public DateTime Insfckfecmodificacion { get; set; }
        public string Insfckusuultimp { get; set; }
        public DateTime Insfckfecultimp { get; set; }
        public string Insfckranfecultimp { get; set; }

        //Additional
        public string Iperinombre { get; set; }
        public string Emprnomb { get; set; }
        public string Equinombcentral { get; set; }
        public string Equinombunidad { get; set; }
        public string Gruponomb { get; set; }
        public string Famnomb { get; set; }
        public string InsfckfecultimpDesc { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndInsumosFactorKDTO
    {

    }

}
