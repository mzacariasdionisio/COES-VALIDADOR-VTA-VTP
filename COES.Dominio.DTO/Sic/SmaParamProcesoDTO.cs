using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_PARAM_PROCESO
    /// </summary>
    public class SmaParamProcesoDTO : EntityBase
    {
        public int Papocodi { get; set; } 
        public string Papohorainicio { get; set; } 
        public string Papohorafin { get; set; } 
        public string Papousucreacion { get; set; } 
        public DateTime? Papofeccreacion { get; set; } 
        public DateTime? Papofecmodificacion { get; set; } 
        public string Papousumodificacion { get; set; } 
        public string Papohoraenvioncp { get; set; } 
       public string Papoestado { get; set; }
       //STS - 19 marzo 2018
       public int Papomaxdiasoferta { get; set; }

        public string PapofeccreacionDesc { get; set; }
    }
}
