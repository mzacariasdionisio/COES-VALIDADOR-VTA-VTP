using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class SiSenializacionDTO
    {
        public int CodigoSenializacion { get; set; }
        public int CodigoEvento { get; set; }
        public string SubEstacion { get; set; }
        public string Equipo { get; set; }
        public string Codigo { get; set; }
        public string Senializaciones { get; set; }
        public string Interruptor { get; set; }
        public string CodigoAC { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
