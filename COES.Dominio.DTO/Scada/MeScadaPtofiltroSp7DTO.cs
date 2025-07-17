using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla ME_SCADA_PTOFILTRO_SP7
    /// </summary>
    public class MeScadaPtofiltroSp7DTO : EntityBase
    {
        public int Scdpficodi { get; set; } 
        public int? Filtrocodi { get; set; } 
        public int? Canalcodi { get; set; } 
        public string Scdpfiusucreacion { get; set; } 
        public DateTime? Scdpfifeccreacion { get; set; } 
        public string Scdpfiusumodificacion { get; set; } 
        public DateTime? Scdpfifecmodificacion { get; set; } 
        public string Filtronomb { get; set; }
        public string Canalnomb { get; set; }
    }
}
