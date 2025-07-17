using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla ME_SCADA_FILTRO_SP7
    /// </summary>
    public class MeScadaFiltroSp7DTO : EntityBase
    {
        public int Filtrocodi { get; set; }        
        public string Filtronomb { get; set; } 
        public string Filtrouser { get; set; } 
        public DateTime? Scdfifeccreacion { get; set; } 
        public string Scdfiusumodificacion { get; set; } 
        public DateTime? Scdfifecmodificacion { get; set; }

        
    }
}
