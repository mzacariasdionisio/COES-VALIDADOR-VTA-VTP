using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class PeriodoDTO
    {
        public int PeriCodigo { get; set; }

        public string PeriNombre { get; set; }

        public DateTime PeriFechaInicio { get; set; }

        public DateTime PeriFechaFin { get; set; }

        public DateTime PeriHoraFin { get; set; }

        public int PeriHorizonteAtras { get; set; }

        public int PeriHorizonteAdelante { get; set; }

        public string PeriComentario { get; set; }

        public string PeriEstado { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }
        public List<DetallePeriodoDTO> ListaDetallePeriodo { get; set; }
    }
}
