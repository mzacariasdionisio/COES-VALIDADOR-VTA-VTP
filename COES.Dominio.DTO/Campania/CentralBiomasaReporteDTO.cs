using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CentralBiomasaReporteDTO
    {
        public List<BioHojaADTO> listaBioA { get; set; }
        public List<BioHojaBDTO> listaBioB { get; set; }
        public List<DetBioHojaCDTO> listaBioC { get; set; }

        public CentralBiomasaReporteDTO()
        {
            listaBioA = new List<BioHojaADTO>();
            listaBioB = new List<BioHojaBDTO>();
            listaBioC = new List<DetBioHojaCDTO>();
        }
    }
}
