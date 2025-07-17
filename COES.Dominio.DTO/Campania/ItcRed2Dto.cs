using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcRed2Dto
    {
        public int ItcRed2Codi { get; set; }
        public int ProyCodi { get; set; }
        public string Linea { get; set; }
        public string BarraE { get; set; }
        public string BarraR { get; set; }
        public decimal? Nternas { get; set; }
        public decimal? Tramo { get; set; }
        public string Electroducto { get; set; }
        public decimal? Longitud { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
