using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1BDetDTO
    {
        public int FormatoD1BDetCodi { get; set; }
        public int FormatoD1BCodi { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public decimal? DemandaEnergia { get; set; }
        public decimal? DemandaHP { get; set; }
        public decimal? DemandaHFP { get; set; }
        public decimal? GeneracionEnergia { get; set; }
        public decimal? GeneracionHP { get; set; }
        public decimal? GeneracionHFP { get; set; }
        public decimal? DemandaNetaHP { get; set; }
        public decimal? DemandaNetaHFP { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
