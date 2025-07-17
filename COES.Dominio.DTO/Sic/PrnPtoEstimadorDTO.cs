using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnPtoEstimadorDTO
    {
        public int Ptoetmcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public string Ptoetmtipomedi { get; set; }
        public string Ptomedidesc { get; set; }
        public int Origlectcodi { get; set; }
    }
}
