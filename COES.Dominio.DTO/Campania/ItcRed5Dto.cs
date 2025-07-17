using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcRed5Dto
    {
        public int ItcRed5Codi { get; set; }
        public int ProyCodi { get; set; }
        public string CaiGen { get; set; }
        public string IdGen { get; set; }
        public string Barra { get; set; }
        public decimal? PdMw { get; set; }
        public decimal? PnMw { get; set; }
        public decimal? QnMin { get; set; }
        public decimal? QnMa { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
