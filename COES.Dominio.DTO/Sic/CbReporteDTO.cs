using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_REPORTE
    /// </summary>
    public partial class CbReporteDTO : EntityBase
    {
        public int Cbrepcodi { get; set; } 
        public DateTime? Cbrepmesvigencia { get; set; } 
        public int? Cbrepversion { get; set; } 
        public string Cbrepnombre { get; set; } 
        public string Cbrepusucreacion { get; set; } 
        public DateTime? Cbrepfeccreacion { get; set; } 
        public string Cbrepusumodificacion { get; set; } 
        public DateTime? Cbrepfecmodificacion { get; set; }
        public int? Cbreptipo { get; set; }
    }

    public partial class CbReporteDTO 
    {
        public List<CbNotaDTO> ListaNotas { get; set; }
        public List<CbRepCabeceraDTO> ListaCabeceras { get; set; }
        public List<CbReporteCentralDTO> ListaCentrales { get; set; }
        public string CbrepfeccreacionDesc { get; set; }
        public string CbrepfecmodificacionDesc { get; set; }
    }
}
