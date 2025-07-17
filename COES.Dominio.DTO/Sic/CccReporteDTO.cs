using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CCC_REPORTE
    /// </summary>
    public partial class CccReporteDTO : EntityBase
    {
        public int Cccrptcodi { get; set; }
        public int Cccvercodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Mogrupocodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public decimal? Cccrptvalorreal { get; set; }
        public decimal? Cccrptvalorteorico { get; set; }
        public decimal? Cccrptvariacion { get; set; }
        public int Cccrptflagtienecurva { get; set; }
    }

    public partial class CccReporteDTO
    {
        public string Emprabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Fenergnomb { get; set; }
        public string Tipoinfoabrev { get; set; }
        public string Mogruponomb { get; set; }
        public DateTime Cccverfecha { get; set; }

        public string CccrptvalorrealDesc { get; set; }
        public string CccrptvalorteoricoDesc { get; set; }
        public string CccrptvariacionDesc { get; set; }
        public bool PintarCeldaFaltaValorReal { get; set; }
        public bool PintarCeldaFaltaValorTeorico { get; set; }
        public string ColorFondo { get; set; }

        public bool TieneTransgresion { get; set; }
        public bool TieneAlertaHo { get; set; }
        public bool TieneAlertaFaltaDataCoes { get; set; }
    }
}
