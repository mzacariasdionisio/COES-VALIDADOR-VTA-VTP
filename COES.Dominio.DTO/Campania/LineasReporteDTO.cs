using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasReporteDTO
    {
        public List<LineasFichaADTO> ListaLineaA { get; set; }
        public List<LineasFichaATramoDTO> ListaLineaATramo { get; set; }
        public List<LineasFichaBDetDTO> ListaLineaB { get; set; }

        public LineasReporteDTO()
        {
            ListaLineaA = new List<LineasFichaADTO>();
            ListaLineaATramo = new List<LineasFichaATramoDTO>();
            ListaLineaB = new List<LineasFichaBDetDTO>();
        }
    }
}
