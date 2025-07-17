using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_MODO_OPER_VAL
    /// </summary>
    public class SmaModoOperValDTO : EntityBase, IComparable<SmaModoOperValDTO>
    {
        public int Mopvcodi { get; set; } 
        public string Mopvusucreacion { get; set; } 
        public DateTime? Mopvfeccreacion { get; set; } 
        public string Mopvusumodificacion { get; set; } 
        public DateTime? Mopvfecmodificacion { get; set; } 
        public int? Mopvgrupoval { get; set; } 
        public int Grupocodi { get; set; } 

        public string Gruponomb { get; set; } 
        public string Grupotipo { get; set; } 

        public string Mopvestado { get; set; }

        public int Urscodi { get; set; }

        public string Ursnomb { get; set; }

        public string MopvListMOVal { get; set; }

        public decimal CapacidadMaxima { get; set; }

        public SmaModoOperValDTO Copy()
        {
            return (SmaModoOperValDTO)this.MemberwiseClone();
        }

        public int CompareTo(SmaModoOperValDTO other)
        {
            return this.CapacidadMaxima.CompareTo(other.CapacidadMaxima);
        }

    }
}
