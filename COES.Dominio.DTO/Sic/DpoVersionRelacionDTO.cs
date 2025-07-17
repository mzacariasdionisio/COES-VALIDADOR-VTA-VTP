using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class DpoVersionRelacionDTO
    {
        public int Dposplcodi { get; set; }
        public string Dposplnombre { get; set; }
        public string Dposplusucreacion { get; set; }
        public DateTime Dposplfeccreacion { get; set; }
        public string Dposplusumodificacion { get; set; }
        public DateTime Dposplfecmodificacion { get; set; }
    }
}
