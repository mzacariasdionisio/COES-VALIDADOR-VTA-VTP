using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CmCompensacionDTO
    {
        public int Compcodi { get; set; }       
        public int Equicodi { get; set; }
        public int Subcausaevencodi { get; set; }
        public DateTime? Compfecha { get; set; }
        public int Compintervalo { get; set; }
        public decimal Compvalor { get; set; }
        public string Compsucreacion { get; set; }
        public DateTime? Compfeccreacion { get; set; }
        public string Compusumodificacion { get; set; }
        public DateTime? Compfecmodificacion { get; set; }
        

    }
}
