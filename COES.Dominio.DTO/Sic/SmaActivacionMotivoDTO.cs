using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_ACTIVACION_MOTIVO
    /// </summary>
    public class SmaActivacionMotivoDTO : EntityBase
    {
        public int Smaacmcodi { get; set; } 
        public int? Smapaccodi { get; set; } 
        public int? Smammcodi { get; set; } 
        public string Smaacmtiporeserva { get; set; } 
        public string Smaacmusucreacion { get; set; } 
        public DateTime? Smaacmfeccreacion { get; set; } 
        public string Smaacmusumodificacion { get; set; } 
        public DateTime? Smaacmfecmodificacion { get; set; } 
    }
}
