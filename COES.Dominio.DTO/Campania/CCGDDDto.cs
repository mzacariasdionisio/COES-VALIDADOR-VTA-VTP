using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CCGDDDTO
    {
        public int CcgddCodi { get; set; }
        public int ProyCodi { get; set; }
        public string Hora { get; set; }
        public decimal? Demanda { get; set; }
        public decimal? Generacion { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
        public string Empresa { get; set; }

        public string NombreProyecto { get; set; }

        public string TipoProyecto { get; set; }

        public string SubTipoProyecto { get; set; }

        public string DetalleProyecto { get; set; }
        public string Confidencial { get; set; }

    }
}
