using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DAI_TABLACODIGO
    /// </summary>
    public class DaiTablacodigoDTO : EntityBase
    {
        public int Tabcodi { get; set; } 
        public string Tabdescripcion { get; set; } 
        public string Tabactivo { get; set; } 
        public string Tabusucreacion { get; set; } 
        public DateTime? Tabfeccreacion { get; set; } 
        public string Tabusumodificacion { get; set; } 
        public DateTime? Tabfecmodificacion { get; set; } 
    }
}
