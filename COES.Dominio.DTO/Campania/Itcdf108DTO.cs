using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdf108DTO
    {
        public int Itcdf108Codi { get; set; }
        public int ProyCodi { get; set; }
        public int Anio { get; set; }
        public decimal? Atval { get; set; }
        public decimal? Mtval { get; set; }
        public decimal? Btval { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string AreaDemanda { get; set; }

        public string Empresa { get; set; }


    }
}
