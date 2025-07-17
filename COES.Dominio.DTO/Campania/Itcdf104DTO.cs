using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdf104DTO
    {
        public int Itcdf104Codi { get; set; }
        public int ProyCodi { get; set; }
        public int Anio { get; set; }
        public decimal? MillonesolesPbi { get; set; }
        public decimal? TasaCrecimientoPbi { get; set; }
        public decimal? NroClientesLibres { get; set; }
        public decimal? NroClientesRegulados { get; set; }
        public decimal? NroHabitantes { get; set; }
        public decimal? TasaCrecimientoPoblacion { get; set; }
        public decimal? MillonesClientesRegulados { get; set; }
        public decimal? ClientesReguladoSelectr { get; set; }
        public decimal? Usmwh { get; set; }
        public decimal? TasaCrecimientoEnergia { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }


        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }

    }
}
