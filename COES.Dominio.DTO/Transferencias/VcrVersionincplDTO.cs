using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_VERSIONINCPL
    /// </summary>
    public class VcrVersionincplDTO : EntityBase
    {
        public int Vcrinccodi { get; set; } 
        public int Pericodi { get; set; } 
        public string Vcrincnombre { get; set; } 
        public string Vcrincestado { get; set; } 
        public string Vcrincusucreacion { get; set; } 
        public DateTime Vcrincfeccreacion { get; set; } 
        public string Vcrincusumodificacion { get; set; } 
        public DateTime Vcrincfecmodificacion { get; set; }

        //agregados para consulta
        public string Perinombre { get; set; }
    }
}
