using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdf116DTO
    {
        public int Itcdf116Codi { get; set; }
        public int ProyCodi { get; set; }
        public string AreaDemanda { get; set; }
        public string Sistema { get; set; }
        public string Subestacion { get; set; }
        public decimal? Tension { get; set; }
        public string Barra { get; set; }
        public string NombreCliente { get; set; }
        public string IdCarga { get; set; }
        public List<Itcdf116DetDTO> ListItcdf116Det { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }
 
    }
}
