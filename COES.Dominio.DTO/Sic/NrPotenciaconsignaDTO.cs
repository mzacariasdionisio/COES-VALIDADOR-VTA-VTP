using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_POTENCIACONSIGNA
    /// </summary>
    public class NrPotenciaconsignaDTO : EntityBase
    {
        public int Nrpccodi { get; set; } 
        public int? Nrsmodcodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public DateTime? Nrpcfecha { get; set; } 
        public string Nrpceliminado { get; set; } 
        public string Nrpcusucreacion { get; set; } 
        public DateTime? Nrpcfeccreacion { get; set; } 
        public string Nrpcusumodificacion { get; set; } 
        public DateTime? Nrpcfecmodificacion { get; set; } 
        public string Nrsmodnombre { get; set; }
        public string Gruponomb { get; set; }
    }
}
