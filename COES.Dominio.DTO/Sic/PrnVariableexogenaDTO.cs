using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{    
    public class PrnVariableexogenaDTO : EntityBase
    {
        public int Varexocodi { get; set; }
        public string Varexonombre { get; set; }
        public string Varexousucreacion { get; set; }
        public DateTime Varexofeccreacion { get; set; }
        public string Varexousumodificacion { get; set; }
        public DateTime Varexofecmodificacion { get; set; }
    }
}
