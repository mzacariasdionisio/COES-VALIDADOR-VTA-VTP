using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_DETFCOSTOF
    /// </summary>
    public class CpDetfcostofDTO : EntityBase
    {
        public int Topcodi { get; set; } 
        public int Detfcfncorte { get; set; } 
        public string Detfcfvalores { get; set; } 
    }
}
