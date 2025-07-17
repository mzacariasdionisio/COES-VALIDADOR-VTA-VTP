using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CmGeneracionDTO
    {
        public int Genecodi { get; set; }
        public int Equicodi { get; set; }
        public DateTime? Genefecha { get; set; }
        public int Geneintervalo { get; set; }
        public decimal Genevalor { get; set; }
        public string Genesucreacion { get; set; }
        public DateTime? Genefeccreacion { get; set; }
        public string Geneusumodificacion { get; set; }
        public DateTime? Genefecmodificacion { get; set; }
        

    }
}
