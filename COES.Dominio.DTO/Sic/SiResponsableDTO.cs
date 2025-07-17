using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class SiResponsableDTO
    {
        public int CodigoResponsable { get; set; }
        public int CodigoDirector { get; set; }
        public string NombreCompleto { get; set; }
        public string Estado { get; set; }
        public string NombreArchivoFirma { get; set; }
        public string Repfirma { get; set; }
    }
}
