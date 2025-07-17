using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcRed1Dto
    {
        public int ItcRed1Codi { get; set; }
        public int ProyCodi { get; set; }
        public string Barra { get; set; }
        public decimal? Vnpu { get; set; }
        public decimal? Vopu { get; set; }
        public string Tipo { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }

    }
}
