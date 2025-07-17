using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_REPORTE
    /// </summary>
    public partial class MeReporteDTO : EntityBase
    {
        public int Reporcodi { get; set; }
        public string Repornombre { get; set; }
        public string Repordescrip { get; set; }
        public int? Lectcodi { get; set; }
        public string Reporusucreacion { get; set; }
        public DateTime? Reporfeccreacion { get; set; }
        public string Reporusumodificacion { get; set; }
        public DateTime? Reporfecmodificacion { get; set; }
        public int? Modcodi { get; set; }
        public int? Cabcodi { get; set; }
        public int? Areacode { get; set; }
        public int Reporcheckempresa { get; set; }
        public int Reporcheckequipo { get; set; }
        public int Reporchecktipoequipo { get; set; }
        public int Reporchecktipomedida { get; set; }
        public string ReporfeccreacionDesc { get; set; }
        public string ReporfecmodificacionDesc { get; set; }
        public int Reptiprepcodi { get; set; }
        public int? Mrepcodi { get; set; }
        public string Reporcolor { get; set; }

        public string Reporejey { get; set; }

        public string Reporesgrafico { get; set; }
        public int IsCheck { get; set; }
    }

    public partial class MeReporteDTO
    {
        public string Modnombre { get; set; }
        public string Areaname { get; set; }
        public string Areaabrev { get; set; }

        public bool EsNorte { get; set; }
        public string NombreArea { get; set; }
        public string TituloTabla { get; set; }
        public string PiePagina { get; set; }
        public string TituloGrafico { get; set; }
        public string SubtituloGrafico { get; set; }
        public int RowIni { get; set; }

        public int Orden { get; set; }
        public string AreaOperativa { get; set; }

        public List<MeReporptomedDTO> ListaPuntos;
        public List<MeMedicion48DTO> ListaMD48;
        public List<MeMedicion48DTO> ListaDetalle48;
    }
}
