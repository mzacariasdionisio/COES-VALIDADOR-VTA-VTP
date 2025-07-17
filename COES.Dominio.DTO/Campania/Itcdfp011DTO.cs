using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdfp011DTO
    {
        public int Itcdfp011Codi { get; set; }
        public int ProyCodi { get; set; }
        public string FechaHora { get; set; }
        public int NroBarras { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }

        public List<Itcdfp011DetDTO> ListItcdf011Det { get; set; }
    }
}
