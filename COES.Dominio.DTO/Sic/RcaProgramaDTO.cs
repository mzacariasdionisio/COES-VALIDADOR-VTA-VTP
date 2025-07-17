using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_PROGRAMA
    /// </summary>
    public class RcaProgramaDTO : EntityBase
    {
        public int Rcprogcodi { get; set; } 
        public string Rcproghorizonte { get; set; } 
        public string Rcprognombre { get; set; }
        public string Rcprogabrev { get; set; }
        public int Rchorpcodi { get; set; } 
        public string Rcprogestregistro { get; set; } 
        public string Rcprogusucreacion { get; set; } 
        public DateTime Rcprogfeccreacion { get; set; } 
        public string Rcprogusumodificacion { get; set; } 
        public DateTime? Rcprogfecmodificacion { get; set; }
        public string Rcprogestado { get; set; }
        public int Rcprogcodipadre { get; set; }

        public DateTime Rcprogfecinicio { get; set; }
        public DateTime Rcprogfecfin { get; set; } 

        public int Nrocuadros { get; set; }

    }
}
