using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_REPA_RECA_PEAJE_DETALLE
    /// </summary>
    public class VtpRepaRecaPeajeDetalleDTO : EntityBase
    {
        
        public int Rrpdcodi { get; set; } 
        public int Rrpecodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal? Rrpdporcentaje { get; set; } 
        public string Rrpdusucreacion { get; set; } 
        public DateTime Rrpdfeccreacion { get; set; } 
        public string Rrpdusumodificacion { get; set; } 
        public DateTime Rrpdfecmodificacion { get; set; }
        //Variables adicionales para mostrar en consultas
        public string Emprnomb { get; set; }
    }
}
