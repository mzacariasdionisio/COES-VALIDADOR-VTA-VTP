using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    ///  Clase que mapea la tabla RCA_HORIZONTE_PROG
    /// </summary>
    public class RcaHorizonteProgDTO : EntityBase
    {
        public int Rchorpcodi { get; set; }
        public string Rchorpnombre { get; set; }
        public string Rchorpestregistro { get; set; }
        public string Rchorpusucreacion { get; set; }
        public DateTime Rchorpfeccreacion { get; set; }
        public string Rchorpusumodificacion { get; set; }
        public DateTime? Rchorpfecmodificacion { get; set; }
        
    }
}
