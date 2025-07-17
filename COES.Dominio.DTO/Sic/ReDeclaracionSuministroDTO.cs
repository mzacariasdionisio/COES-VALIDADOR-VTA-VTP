using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_DECLARACION_SUMINISTRO
    /// </summary>
    public class ReDeclaracionSuministroDTO : EntityBase
    {
        public int Redeccodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Reenvcodi { get; set; } 
        public string Redeindicador { get; set; } 
    }
}
