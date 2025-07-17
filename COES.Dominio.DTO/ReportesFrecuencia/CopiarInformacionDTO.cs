using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class CopiarInformacionDTO
    {
        public int IdCopia { get; set; }
        public int GPSCodiOrigen { get; set; }
        public int GPSCodiDest { get; set; }
        public string GPSDescOrigen { get; set; }
        public string GPSDescDestino { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public string FecHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string FecHoraFin { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public DateTime? FecCreacion { get; set; }
        public string FecCrea { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FecElimina { get; set; }
        public string FechaElimina { get; set; }
        public string UsuarioElimina { get; set; }
        public string Mensaje { get; set; }
        public int Resultado { get; set; }
    }
}
