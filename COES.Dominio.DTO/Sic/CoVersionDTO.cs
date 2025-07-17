using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_VERSION
    /// </summary>
    public partial class CoVersionDTO : EntityBase
    {
        public int Covercodi { get; set; } 
        public int? Copercodi { get; set; } 
        public string Coverdesc { get; set; } 
        public DateTime? Coverfecinicio { get; set; } 
        public DateTime? Coverfecfin { get; set; } 
        public string Coverestado { get; set; } 
        public string Covertipo { get; set; } 
        public int? Covebacodi { get; set; } 
        public string Coverusucreacion { get; set; } 
        public DateTime? Coverfeccreacion { get; set; } 
        public string Coverusumodificacion { get; set; } 
        public DateTime? Coverfecmodificacion { get; set; } 
        public string Destipo { get; set; }
        public string ListaURS { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }

    public partial class CoVersionDTO
    {
        public string Coperestado { get; set; }
    }
}
