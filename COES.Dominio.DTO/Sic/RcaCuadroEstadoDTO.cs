using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class RcaCuadroEstadoDTO : EntityBase
    {
        public int Rcestacodi { get; set; }
        public string Rcestanombre { get; set; }
        public string Rccestaestregistro { get; set; }
        public string Rcestausucreacion { get; set; }
        public DateTime Rcestafeccreacion { get; set; }
        public string Rcestausumodificacion { get; set; }
        public DateTime? Rcestafecmodificacion { get; set; }
       
    }
}
