using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_PROCESO_DIARIO
    /// </summary>
    public partial class CoProcesoDiarioDTO : EntityBase
    {
        public int Prodiacodi { get; set; } 
        public DateTime? Prodiafecha { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Perprgcodi { get; set; } 
        public int? Covercodi { get; set; } 
        public string Prodiaindreproceso { get; set; } 
        public string Prodiatipo { get; set; } 
        public string Prodiaestado { get; set; } 
        public string Prodiausucreacion { get; set; } 
        public DateTime? Prodiafeccreacion { get; set; } 
        public string Prodiausumodificacion { get; set; } 
        public DateTime? Prodiafecmodificacion { get; set; } 
    }

    public partial class CoProcesoDiarioDTO
    {
        public string ProdiafechaDesc { get; set; }
        public string ProdiafecmodificacionDesc { get; set; }
        public string ProdiaindreprocesoDesc { get; set; }
        

    }
}
