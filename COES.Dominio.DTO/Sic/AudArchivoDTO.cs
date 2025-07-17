using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_ARCHIVO
    /// </summary>
    public class AudArchivoDTO : EntityBase
    {
        public int Archcodi { get; set; } 
        public string Archnombre { get; set; } 
        public string Archruta { get; set; } 
        public string Archactivo { get; set; } 
        public string Archusucreacion { get; set; } 
        public DateTime? Archfechacreacion { get; set; } 
        public string Archusumodificacion { get; set; } 
        public DateTime? Archfechamodificacion { get; set; } 
    }
}
