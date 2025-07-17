using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_TABLACODIGO
    /// </summary>
    public class AudTablacodigoDTO : EntityBase
    {
        public int Tabccodi { get; set; } 
        public string Tabcdescripcion { get; set; } 
        public string Tabcactivo { get; set; } 
        public string Tabcusucreacion { get; set; } 
        public DateTime? Tabcfeccreacion { get; set; } 
        public string Tabcusumodificacion { get; set; } 
        public DateTime? Tabcfecmodificacion { get; set; } 
    }
}
