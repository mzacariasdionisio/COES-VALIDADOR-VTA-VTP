using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    class GmmRegistroDTO : EntityBase
    {
        public int REGICODI { get; set; }
        public DateTime? REGIFECREGISTRO { get; set; }
        public string REGIEDITADOPORSTR { get; set; }
        public int EMPGCODI { get; set; }
        public int PERICODI { get; set; }
        public string REGIESTADO { get; set; }
        public string REGIUSUCREACION { get; set; }
        public DateTime? REGIFECCREACION { get; set; }
        public string REGIUSUMODIFICACION { get; set; }
        public DateTime? REGIFECMODIFICACION { get; set; }

    }
}
