using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_TABLACODIGO_DETALLE
    /// </summary>
    public class AudTablacodigoDetalleDTO : EntityBase
    {
        public int Tabcdcodi { get; set; } 
        public int Tabccodi { get; set; } 
        public string Tabcddescripcion { get; set; }
        public string Tabcdvalor { get; set; }
        public int Tabcdorden { get; set; }
        public string Tabcdactivo { get; set; } 
        public string Tabcdusucreacion { get; set; } 
        public DateTime? Tabcdfeccreacion { get; set; } 
        public string Tabcdusumodificacion { get; set; } 
        public DateTime? Tabcdfecmodificacion { get; set; } 
    }
}
