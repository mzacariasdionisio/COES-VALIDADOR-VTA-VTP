using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcPrm2Dto
    {
        public int ItcPrm2Codi { get; set; }
        public int ProyCodi { get; set; }
        public string Transformador { get; set; }
        public string Tipo { get; set; }
        public decimal? Fases { get; set; }
        public decimal? Ndvn { get; set; }
        public decimal? Vnp { get; set; }
        public decimal? Vns { get; set; }
        public decimal? Vnt { get; set; }
        public string Pnp { get; set; }
        public string Pns { get; set; }
        public string Pnt { get; set; }
        public decimal? Tccps { get; set; }
        public decimal? Tccst { get; set; }
        public decimal? Tcctp { get; set; }
        public decimal? Pcups { get; set; }
        public decimal? Pcust { get; set; }
        public decimal? Pcutp { get; set; }
        public decimal? Pfe { get; set; }
        public decimal? Ivacio { get; set; }
        public string Grpcnx { get; set; }
        public string Taptipo { get; set; }
        public string Taplado { get; set; }
        public decimal? Tapdv { get; set; }
        public decimal? Tapmin { get; set; }
        public decimal? Tapcnt { get; set; }
        public decimal? Tapmax { get; set; }

        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
