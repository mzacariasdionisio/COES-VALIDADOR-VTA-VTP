using COES.Base.Core;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_DATOS
    /// </summary>
    public partial class CbDatosDTO : EntityBase
    {
        public int Cbevdacodi { get; set; }
        public int? Cbcentcodi { get; set; }
        public int Cbvercodi { get; set; }
        public int Ccombcodi { get; set; }

        public string Cbevdavalor { get; set; }
        public string Cbevdatipo { get; set; }
        public string Cbevdavalor2 { get; set; }
        public string Cbevdatipo2 { get; set; }

        public int? Cbevdaconfidencial { get; set; }
        public int? Cbevdaestado { get; set; }
        public int? Cbevdanumdecimal { get; set; }
    }

    public partial class CbDatosDTO
    {
        public int Equicodi { get; set; }

        public decimal Valor { get; set; }
        public decimal Valor2 { get; set; }
        public bool TieneError { get; set; }

        public string FormulaValor { get; set; }
        public string FormulaValor2 { get; set; }

        public string CeldaExcel { get; set; }
        public string Cbevdavalornumerico { get; set; }

        public List<CbDatosDetalleDTO> ListaDetalle { get; set; }
        public CbObsDTO Obs { get; set; }

        public int? ValorEntero { get; set; }

        public int PosColExcel { get; set; }
        public int PosRowExcel { get; set; }
    }
}
