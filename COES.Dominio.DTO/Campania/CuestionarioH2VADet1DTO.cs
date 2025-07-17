using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CuestionarioH2VADet1DTO
    {
        public int H2vaDet1Codi { get; set; }
        public int H2vaCodi { get; set; }
        public int? Anio { get; set; }
        public decimal? MontoInversion { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
