using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcPrm1Dto
    {
        public int ItcPrm1Codi { get; set; }
        public int ProyCodi { get; set; }
        public string Electroducto { get; set; }
        public string Descripcion { get; set; }
        public decimal? Vn { get; set; }
        public string Tipo { get; set; }
        public decimal? Seccion { get; set; }
        public decimal? Ctr { get; set; }
        public decimal? R { get; set; }
        public decimal? X { get; set; }
        public decimal? B { get; set; }
        public decimal? Ro { get; set; }
        public decimal? Xo { get; set; }
        public decimal? Bo { get; set; }
        public decimal? Capacidad { get; set; }
        public decimal? Tmxop { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
