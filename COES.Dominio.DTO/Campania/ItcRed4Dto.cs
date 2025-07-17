using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ItcRed4Dto
    {
        public int ItcRed4Codi { get; set; }
        public int ProyCodi { get; set; }
        public string IdCmp { get; set; }
        public string Barra { get; set; }
        public string Tipo { get; set; }
        public decimal? Vnkv { get; set; }
        public decimal? CapmVar { get; set; }
        public decimal? Npasos { get; set; }
        public decimal? PasoAct { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }


    }
}
