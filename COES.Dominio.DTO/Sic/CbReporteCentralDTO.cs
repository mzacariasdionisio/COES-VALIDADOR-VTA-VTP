using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_REPORTE_CENTRAL
    /// </summary>
    public partial class CbReporteCentralDTO : EntityBase
    {
        public int Cbrepcodi { get; set; } 
        public int Cbrcencodi { get; set; } 
        public int Cbcentcodi { get; set; } 
        public string Cbrcennombre { get; set; }
        public int Cbrcencoloreado { get; set; }
        public int Cbrcenorigen { get; set; } // 1:Del calculo, 2:De asignacion, 3:Copiado mes pasado
        public int Cbrcenorden { get; set; }
    }

    public partial class CbReporteCentralDTO
    {
        public List<CbReporteDetalleDTO> ListaDetalles { get; set; }
        public int Equicodi { get; set; }
        public int? OrdenEnLista { get; set; }
    }
}
