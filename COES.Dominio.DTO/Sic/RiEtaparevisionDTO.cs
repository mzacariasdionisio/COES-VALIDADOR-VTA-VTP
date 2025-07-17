using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_ETAPAREVISION
    /// </summary>
    public class RiEtaparevisionDTO : EntityBase
    {
        public int Etrvcodi { get; set; } 
        public string Etrvnombre { get; set; } 
        public string Etrvestado { get; set; } 
        public string Etrvusucreacion { get; set; } 
        public DateTime? Etrvfeccreacion { get; set; } 
        public string Etrvusumodificacion { get; set; } 
        public DateTime? Etrvfecmodificacion { get; set; } 
    }
}
