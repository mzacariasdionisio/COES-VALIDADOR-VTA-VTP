using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RespuestaObsDTO
    {
        public int RespuestaId { get; set; }

        public int ObservacionId { get; set; }

        public DateTime? FechaRespuesta { get; set; }

        public string Respuesta { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }

    }
}
