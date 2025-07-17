using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.SGDoc
{
    public class ReferenciaDTO
    {
        public int Correlativo { get; set; }
        public DateTime FechaDoc { get; set; }
        public string NDoc { get; set; }
        public string Asunto { get; set; }
        public string RutaArchivo { get; set; }
    }
}
