using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcRed3Dto
    {
        public int ItcRed3Codi { get; set; }
        public int ProyCodi { get; set; }
        public string IdCircuito { get; set; }
        public string BarraP { get; set; }
        public string BarraS { get; set; }
        public string BarraT { get; set; }
        public string CdgTrafo { get; set; }
        public string OprTap { get; set; }
        public decimal? PosTap { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
