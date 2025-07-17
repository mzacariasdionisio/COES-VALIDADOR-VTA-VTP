using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdf110DetDTO
    {
        public int Itcdf110DetCodi { get; set; }
        public int Itcdf110Codi { get; set; }
        public int Anio { get; set; }
        public decimal? Valor { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
