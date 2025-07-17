using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.ReportesFrecuencia
{
    public class ExtraerFrecuenciaDTO
    {
        public int IdCarga { get; set; }
        public int GPSCodi { get; set; }
        public string GPSNombre { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string ArchivoCarga { get; set; }
        public string DataCarga { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string UsuCreacion { get; set; }
        public string NombreEquipo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Resultado { get; set; }
        public string RutaFile { get; set; }
        public string FechaHoraInicioString { get; set; }
        public string FechaHoraFinString { get; set; }
        public string FechaCreacionString { get; set; }
    }
}
