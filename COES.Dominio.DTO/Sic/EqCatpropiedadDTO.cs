using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_CATPROPIEDAD
    /// </summary>
    public class EqCatpropiedadDTO : EntityBase
    {
        public int Eqcatpcodi { get; set; } 
        public string Eqcatpnomb { get; set; } 
        public string Eqcatpusucreacion { get; set; } 
        public DateTime? Eqcatpfeccreacion { get; set; } 
        public string Eqcatpusumodificacion { get; set; } 
        public DateTime? Eqcatpfecmodificacion { get; set; } 
        public string Eqcatpestado { get; set; } 
        public int Ctgcodi { get; set; } 
    }
}
