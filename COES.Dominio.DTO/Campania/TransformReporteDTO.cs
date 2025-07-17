using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class TransformReporteDTO
    {
        public List<T2SubestFicha1DTO> ListaT2Subest1 { get; set; }
        public List<T2SubestFicha1TransDTO> ListaT2Subest1Trans { get; set; }
        public List<T2SubestFicha1EquiDTO> ListaT2Subest1Equi { get; set; }
        public List<CroFicha1DetDTO> ListaT3Crono { get; set; }

        public TransformReporteDTO()
        {
            ListaT2Subest1 = new List<T2SubestFicha1DTO>();
            ListaT2Subest1Trans = new List<T2SubestFicha1TransDTO>();
            ListaT2Subest1Equi = new List<T2SubestFicha1EquiDTO>();
            ListaT3Crono = new List<CroFicha1DetDTO>();
        }
    }
}
