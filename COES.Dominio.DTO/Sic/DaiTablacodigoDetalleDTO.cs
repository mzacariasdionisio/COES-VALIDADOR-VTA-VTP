using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DAI_TABLACODIGO_DETALLE
    /// </summary>
    public class DaiTablacodigoDetalleDTO : EntityBase
    {
        public int Tabdcodi { get; set; } 
        public int Tabcodi { get; set; } 
        public string Tabddescripcion { get; set; } 
        public string Tabdvalor { get; set; } 
        public int? Tabdorden { get; set; } 
        public string Tabdactivo { get; set; } 
        public string Tabdusucreacion { get; set; } 
        public DateTime? Tabdfeccreacion { get; set; } 
        public string Tabdusumodificacion { get; set; } 
        public DateTime? Tabdfecmodificacion { get; set; } 
    }
}
