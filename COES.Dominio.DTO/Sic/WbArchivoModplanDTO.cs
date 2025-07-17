using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_ARCHIVO_MODPLAN
    /// </summary>
    public class WbArchivoModplanDTO : EntityBase
    {
        public int Arcmplcodi { get; set; } 
        public int Vermplcodi { get; set; } 
        public string Arcmplnombre { get; set; } 
        public string Arcmpldesc { get; set; }
        public string Arcmplindtc { get; set; } 
        public string Arcmplestado { get; set; } 
        public string Arcmplext { get; set; }
        public int? Arcmpltipo { get; set; }
    }
}
