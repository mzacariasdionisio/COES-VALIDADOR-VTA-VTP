using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public class VtpPagoEgresoDTO : EntityBase
    {
        public int Pagegrcodi { get; set; }
        public int Pericodi { get; set; }
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Pagegregreso { get; set; }
        public decimal Pagegrsaldo { get; set; }
        public decimal Pagegrpagoegreso { get; set; }
        public string Pagegrusucreacion { get; set; }
        public DateTime Pagegrfeccreacion { get; set; }
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb { get; set; }
    }
}
