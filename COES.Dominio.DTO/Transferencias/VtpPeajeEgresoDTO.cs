using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EGRESO
    /// </summary>
    public class VtpPeajeEgresoDTO : EntityBase
    {
        public int Pegrcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; } 
        public int? Emprcodi { get; set; }
        public string Pegrestado { get; set; }
        public string Pegrplazo { get; set; }
        public string Pegrusucreacion { get; set; } 
        public DateTime Pegrfeccreacion { get; set; }
        public string Pegrusumodificacion { get; set; }
        public DateTime Pegrfecmodificacion { get; set; }
        public string Emprnomb { get; set; }
    }
}
