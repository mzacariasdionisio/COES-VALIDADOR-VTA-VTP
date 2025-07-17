using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class DpoBarraDTO
    {
        public int Dpobarcodi { get; set; }
        public string Dpobarcodiexcel { get; set; }
        public string Dpobarnombre { get; set; }
        public decimal Dpobartension { get; set; }
        public string Dpobarusucreacion { get; set; }
        public DateTime   Dpobarfeccreacion { get; set; }
    }
}
