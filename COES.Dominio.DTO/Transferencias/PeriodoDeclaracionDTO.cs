using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class PeriodoDeclaracionDTO
    {
        public int PeridcCodi { get; set; }
        public int? PeridcAnio { get; set; }
        public int? PeridcMes { get; set; }
        public string PeridcNombre { get; set; }
        public string PeridcAnioMes { get; set; }
        public string Mensaje { get; set; }
        public DateTime? PeridcFecRegi { get; set; }
        public string PeridcUsuarioRegi { get; set; }
        public string PeridcEstado { get; set; }
        public string EstdAbrev { get; set; }
        public int? PeridcNuevo { get; set; }
 
    }
}
