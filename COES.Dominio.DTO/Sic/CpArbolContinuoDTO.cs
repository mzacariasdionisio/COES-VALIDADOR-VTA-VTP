using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_ARBOL_CONTINUO
    /// </summary>
    public class CpArbolContinuoDTO : EntityBase
    {
        public int Cparbcodi { get; set; } 
        public string Cparbtag { get; set; } 
        public DateTime Cparbfecregistro { get; set; } 
        public int Topcodi { get; set; } 
        public string Cparbusuregistro { get; set; } 
        public string Cparbestado { get; set; } 
        public DateTime Cparbfecha { get; set; } 
        public int Cparbbloquehorario { get; set; } 
        public string Cparbdetalleejec { get; set; } 
        public string Cparbidentificador { get; set; } 
        public DateTime? Cparbfeciniproceso { get; set; } 
        public DateTime? Cparbfecfinproceso { get; set; } 
        public string Cparbmsjproceso { get; set; } 
        public decimal Cparbporcentaje { get; set; }

        public List<CpNodoContinuoDTO> ListaNodos { get; set; }
    }
}
