using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ArchivoBaseDTO
    {
        public int id { get; set; }
        public string archivoBase64 { get; set; }
        public string nombreArchivo { get; set; }
    }
}
