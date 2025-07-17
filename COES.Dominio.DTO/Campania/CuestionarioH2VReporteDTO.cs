using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CuestionarioH2VReporteDTO
    {
        public List<CuestionarioH2VADTO> dataH2VA { get; set; }

        public List<CuestionarioH2VBDTO> dataH2VB { get; set; }
        public List<CuestionarioH2VCDTO> dataH2VC { get; set; }
        public List<CuestionarioH2VEDTO> dataH2VE { get; set; }
        public List<CuestionarioH2VFDTO> dataH2VF { get; set; }
      
        public CuestionarioH2VReporteDTO()
        {
            dataH2VA = new List<CuestionarioH2VADTO>();
            dataH2VB = new List<CuestionarioH2VBDTO>();
            // ListaDistC1 = new List<CuestionarioH2VADTO>();
            dataH2VC = new List<CuestionarioH2VCDTO>();
            dataH2VE = new List<CuestionarioH2VEDTO>();
            dataH2VF = new List<CuestionarioH2VFDTO>();
        }
    }
}
