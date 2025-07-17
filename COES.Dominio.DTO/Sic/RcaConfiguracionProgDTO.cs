using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class RcaConfiguracionProgDTO : EntityBase
    {
        public int Rcconpcodi { get; set; }
        public string Rcconpnombre { get; set; }
        public string Rcconpestregistro { get; set; }
        public string Rcconpusucreacion { get; set; }
        public DateTime Rcconpfeccreacion { get; set; }
        public string Rcconpusumodificacion { get; set; }
        public DateTime? Rcconpfecmodificacion { get; set; }
       
    }
}
