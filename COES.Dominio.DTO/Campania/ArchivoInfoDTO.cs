using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ArchivoInfoDTO
    {
        public int ArchCodi { get; set; }

        public int SeccCodi { get; set; }
        public int ProyCodi { get; set; }

        public string ArchNombre { get; set; }

        public string ArchNombreGenerado { get; set; }

        public string ArchTipo {  get; set; }

        public string ArchUbicacion { get; set; }

        public string Descripcion { get; set; }

        public DateTime? ArchFechaSubida { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }

    }
}
