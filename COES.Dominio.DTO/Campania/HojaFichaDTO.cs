using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class HojaFichaDTO
    {
        public int HojaCodigo { get; set; }

        public int TipoFichaCodigo { get; set; }

        public string HojaNombre { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }

        public string UsuarioDel { get; set; }

        public DateTime FechaDel { get; set; }

        public int Orden { get; set; }

    }
}
