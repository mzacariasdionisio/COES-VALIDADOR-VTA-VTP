using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class InfoFaltanteDTO
    {
        public System.Int32 Codi { get; set; }
        public System.String Empresa { get; set; }
        public System.String Barra { get; set; }
        public System.String Cliente { get; set; }        
        public System.String Codigo { get; set; }

        public System.Int32 EmprCodi { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public System.String Tipo { get; set; }




    }
}
