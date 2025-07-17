using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_REPA_RECA_PEAJE
    /// </summary>
    public class VtpRepaRecaPeajeDTO : EntityBase
    {
        public int Rrpecodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; } 
        public string Rrpenombre { get; set; } 
        public string Rrpeusucreacion { get; set; } 
        public DateTime Rrpefeccreacion { get; set; } 
        public string Rrpeusumodificacion { get; set; } 
        public DateTime Rrpefecmodificacion { get; set; } 
    }
}
