using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdf123DTO
    {
        public int Itcdf123Codi { get; set; }
        public int ProyCodi { get; set; }
        public string UtmEste { get; set; }
        public string UtmNorte { get; set; }
        public string UtmZona { get; set; }
        public decimal? Anio1 { get; set; }
        public decimal? Anio2 { get; set; }
        public decimal? Anio3 { get; set; }
        public decimal? Anio4 { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }
    }
}
