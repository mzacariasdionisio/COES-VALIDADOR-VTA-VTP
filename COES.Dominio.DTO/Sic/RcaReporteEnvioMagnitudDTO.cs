using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class RcaReporteEnvioMagnitudDTO
    {
        public string NombreSuministrador { get; set; }
        public string ClienteLibre { get; set; }
        public string Subestacion { get; set; }
        public string NombrePuntoMedicion { get; set; }
        public int DemandaHfp { get; set; }
        public decimal RechazoCargaProgramado { get; set; }
        public decimal RechazoCargaPreliminar { get; set; }
        public DateTime? FechaHoraInicioPreliminar { get; set; }
        public DateTime? FechaHoraFinPreliminar { get; set; }
        public decimal RechazoCargaFinal { get; set; }
        public DateTime? FechaHoraInicioFinal { get; set; }
        public DateTime? FechaHoraFinFinal { get; set; }
        public string Cumplio { get; set; }
    }
}
