using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcSistemaReporteDTO
    {
        public List<ItcPrm1Dto> listaprm1 { get; set; }
        public List<ItcPrm2Dto> listaprm2 { get; set; }
        public List<ItcRed1Dto> listared1 { get; set; }
        public List<ItcRed2Dto> listared2 { get; set; }
        public List<ItcRed3Dto> listared3 { get; set; }
        public List<ItcRed4Dto> listared4 { get; set; }
        public List<ItcRed5Dto> listared5 { get; set; }


        public ItcSistemaReporteDTO()
        {
            listaprm1 = new List<ItcPrm1Dto>();
            listaprm2 = new List<ItcPrm2Dto>();
            listared1 = new List<ItcRed1Dto>();
            listared2 = new List<ItcRed2Dto>();
            listared3 = new List<ItcRed3Dto>();
            listared4 = new List<ItcRed4Dto>();
            listared5 = new List<ItcRed5Dto>();
        }
    }
}
