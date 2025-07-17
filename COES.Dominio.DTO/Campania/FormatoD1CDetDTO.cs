using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1CDetDTO
    {
        public int FormatoD1CDetCodi { get; set; }
        public int FormatoD1CCodi { get; set; }
        public string Hora { get; set; }
        public decimal? Demanda { get; set; }
        public decimal? Generacion { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
