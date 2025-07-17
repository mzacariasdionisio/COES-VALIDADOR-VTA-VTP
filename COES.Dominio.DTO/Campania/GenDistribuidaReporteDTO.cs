using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class GenDistribuidaReporteDTO
    {
        public List<CCGDADTO> ListaDistA { get; set; }
        public List<CCGDBDTO> ListaDistB { get; set; }
        public List<CCGDCOptDTO> ListaDistC1 { get; set; }
        public List<CCGDCPesDTO> ListaDistC2 { get; set; }
        public List<CCGDCDTO> ListaDistC { get; set; }
        public List<CCGDDDTO> ListaDistD { get; set; }
        public List<CCGDEDTO> ListaDistE { get; set; }


        public GenDistribuidaReporteDTO()
        {
            ListaDistA = new List<CCGDADTO>();
            ListaDistB = new List<CCGDBDTO>();
            ListaDistC1 = new List<CCGDCOptDTO>();
            ListaDistC2 = new List<CCGDCPesDTO>();
            ListaDistC = new List<CCGDCDTO>();
            ListaDistD = new List<CCGDDDTO>();
            ListaDistE = new List<CCGDEDTO>();
        }
    }
}
