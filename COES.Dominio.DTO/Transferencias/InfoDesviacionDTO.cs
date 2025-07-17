using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class InfoDesviacionDTO
    {
        public System.String Codigo { get; set; }
        public System.Int32 GenEmprCodi { get; set; }
        public System.String Generador { get; set; }
        public System.Int32 CliEmprCodi { get; set; }
        public System.String Cliente { get; set; }
        public System.Decimal NroDia { get; set; }
        public System.Decimal Energia { get; set; }
        public System.Decimal EnergiaAnterior { get; set; }
        public decimal Desviacion { get; set; }
    }
}
