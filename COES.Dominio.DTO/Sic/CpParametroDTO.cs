using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_PARAMETRO
    /// </summary>
    public class CpParametroDTO : EntityBase
    {
        public int Paramcodi { get; set; } 
        public string Paramnombre { get; set; } 
        public string Paramunidad { get; set; } 
        public string Paramvalor { get; set; } 
        public int Topcodi { get; set; } 
        public int? Paramactivo { get; set; } 
    }
}
