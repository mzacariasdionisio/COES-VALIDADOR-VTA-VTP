using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    class GmmPeriodoDTO : EntityBase
    {
        public int PERICODI { get; set; }
        public int PERIANIO { get; set; }
        public string PERIESTADO { get; set; }
        public string PERIMES { get; set; }
        public string PERIUSUCREACION { get; set; }
        public DateTime? PERIFECCREACION { get; set; }
        public string PERIUSUMODIFICACION { get; set; }
        public DateTime? PERIFECMODIFICACION { get; set; }
      
    }
}
