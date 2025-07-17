using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ArchivoObsDTO
    {
        public int ArchivoId { get; set; }

        public int ObservacionId { get; set; }

        public string NombreArch { get; set; }

        public string RutaArch { get; set; }

        public string Tipo { get; set; }

        public string NombreArchGen {  get; set; }

        public DateTime? ArchFechaSubida { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string IndDel { get; set; }

    }
}
