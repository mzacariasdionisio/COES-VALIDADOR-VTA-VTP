using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class DpoBarraSplDTO
    {
        public int Barsplcodi { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public string   Barsplestado { get; set; }
        public string Barsplusucreacion { get; set; }
        public DateTime Barsplfeccreacion { get; set; }
    }
}
