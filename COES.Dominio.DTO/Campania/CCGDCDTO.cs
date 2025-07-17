using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CCGDCDTO
    {
        public int ProyCodi { get; set; }
        public int Anio { get; set; }
        public decimal? DemandaEnergia { get; set; }
        public decimal? DemandaHP { get; set; }
        public decimal? DemandaHFP { get; set; }
        public decimal? GeneracionEnergia { get; set; }
        public decimal? GeneracionHP { get; set; }
        public decimal? GeneracionHFP { get; set; }

        public string Empresa { get; set; }
        public string NombreProyecto { get; set; }
        public string Escenario { get; set; }
    }
}
