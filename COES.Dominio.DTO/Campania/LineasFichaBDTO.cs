using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class LineasFichaBDTO
    {
        public int FichaBCodi { get; set; }
        public int ProyCodi { get; set; }
        public DateTime? FecPuestaOpe { get; set; }
        public List<LineasFichaBDetDTO> LineasFichaBDetDTO { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
    }
}
