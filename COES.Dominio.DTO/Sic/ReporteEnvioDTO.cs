using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class ReporteEnvioDTO
    {
        public DateTime Fecha { get; set; }
        public int PtoMediCodi { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string IndConsistencia { get; set; }
        public decimal ValConsistencia { get; set; }
        public string EstadoOperativo { get; set; }
        public string EstadoInformacion { get; set; }
    }
}

