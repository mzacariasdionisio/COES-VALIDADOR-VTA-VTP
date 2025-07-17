using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class FrecuenciasAudit
    {
        public int Id { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime Fecha { get; set; }
        public int Registros { get; set; }
        public string  Usuario { get; set; }
        public DateTime? FechaReversa { get; set; }
        public string UsuarioReversa { get; set; }
        public int IdGPS { get; set; }
        public string FechaInicialString { get; set; }
        public string FechaFinalString { get; set; }
        public string FechaString { get; set; }
        public string FechaReversaString { get; set; }
    }
}
