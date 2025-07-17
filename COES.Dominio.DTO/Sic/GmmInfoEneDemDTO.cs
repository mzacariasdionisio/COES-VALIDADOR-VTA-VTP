using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    class GmmInfoEneDemDTO : EntityBase
    {
        public int ENDECODI { get; set; }
        public string ENDETIPO { get; set; }
        public int ENDEANIO { get; set; }
        public string ENDEMES { get; set; }
        public int REGICODI { get; set; }
        public string ENDEUSUCREACION { get; set; }
        public DateTime? ENDEFECCREACION { get; set; }
        public string ENDEUSUMODIFICACION { get; set; }
        public DateTime? ENDEFECMODIFICACION { get; set; }
        
    }
}
