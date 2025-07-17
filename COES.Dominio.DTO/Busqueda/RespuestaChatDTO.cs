using COES.Dominio.DTO.SGDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Busqueda
{
    public class RespuestaChatDTO
    {
        public string Contenido { get; set; }
        public List<ReferenciaChatDTO> Referencias { get; set; }
    }
}
