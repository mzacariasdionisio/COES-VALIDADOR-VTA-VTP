using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_VERSION_MODPLAN
    /// </summary>
    public class WbVersionModplanDTO : EntityBase
    {
        public int Vermplcodi { get; set; } 
        public string Vermpldesc { get; set; } 
        public string Vermplestado { get; set; } 
        public int? Vermplpadre { get; set; } 
        public string Vermplusucreacion { get; set; } 
        public DateTime? Vermplfeccreacion { get; set; } 
        public string Vermplusumodificacion { get; set; } 
        public DateTime? Vermplfecmodificacion { get; set; } 
        public int? Vermpltipo { get; set; }
        public string RutaModelo { get; set; }
        public string RutaManual { get; set; }
        public List<WbArchivoModplanDTO> ListadoArchivos { get; set; }
        
    }
}
