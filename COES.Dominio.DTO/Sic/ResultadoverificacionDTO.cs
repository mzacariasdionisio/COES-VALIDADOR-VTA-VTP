using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class ResultadoverificacionDTO
    {
        public int PtoMediCodi { get; set; }
        public string IndicadorEnvio { get; set; }
        public DateTime FechaCarga { get; set; }
        public string IndicadorConsistencia { get; set; }
        public decimal ValorConsistencia { get; set; }
        public string EstadoOperativo { get; set; }
        public string EstadoInformacion { get; set; }
    }
}

