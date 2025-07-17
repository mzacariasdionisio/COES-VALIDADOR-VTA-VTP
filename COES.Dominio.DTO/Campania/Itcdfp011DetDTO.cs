using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdfp011DetDTO
    {
        public int Itcdfp011DetCodi { get; set; }
        public int Itcdfp011Codi { get; set; }
        public string FechaHora { get; set; }
        public int BarraNro { get; set; }
        public decimal? Kwval { get; set; }
        public decimal? Kvarval { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
