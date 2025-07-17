using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_REPORTE_DETALLE
    /// </summary>
    public class CbReporteDetalleDTO : EntityBase
    {
        public int Cbrepdcodi { get; set; } 
        public int Ccombcodi { get; set; } 
        public int Cbrcencodi { get; set; } 
        public string Cbrepdvalor { get; set; } 
        public decimal? Cbrepvalordecimal { get; set; } 
    }
}
