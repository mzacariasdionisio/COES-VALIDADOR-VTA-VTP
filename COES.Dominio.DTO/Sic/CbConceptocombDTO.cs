using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_CONCEPTOCOMB
    /// </summary>
    public partial class CbConceptocombDTO : EntityBase
    {
        public int Ccombcodi { get; set; }
        public int Estcomcodi { get; set; }
        public string Ccombnombre { get; set; }
        public string Ccombnombreficha { get; set; }
        public string Ccombunidad { get; set; }
        public int Ccomborden { get; set; }
        public string Ccombabrev { get; set; }
        public string Ccombreadonly { get; set; }
        public int Ccombnumeral { get; set; }
        public int Ccombtunidad { get; set; }
        public int Ccombseparador { get; set; }
        public int Ccombnumdecimal { get; set; }
        public string Ccombtipo { get; set; }
        public int Ccombtipo2 { get; set; }
        public int Ccombunidad2 { get; set; }
        public string Ccombobligatorio { get; set; }
        public int Ccombestado { get; set; }
    }

    public partial class CbConceptocombDTO 
    {
        public string Numeral { get; set; }
        public CbDatosDTO ItemDato { get; set; }

        public string Moneda { get; set; }
        public string MonedaDesc { get; set; }
        public string MonedaXUnidad { get; set; }
        public bool EsObligatorioTC { get; set; }

        public int ConcepcodiCombAlmacenado { get; set; }
        public int ConcepcodiVolCombAlmacenado { get; set; }
        public int ConcepcodiFactura { get; set; }
        public int ConcepcodiDemurrage { get; set; }
        public int ConcepcodiMerma { get; set; }
        public bool OpcionalDemurrage { get; set; }
        public bool OpcionalFactura { get; set; }
        public bool OpcionalMerma { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public string FechaIngreso { get; set; }
        public decimal CombustibleAlmacenado { get; set; }

        public string Procedencia { get; set; }
        public string Proveedor { get; set; }
        public string Sitio { get; set; }
        public string Sitio2 { get; set; }
        public string Puerto { get; set; }
        public string Puerto2 { get; set; }
        public decimal PcSup { get; set; }
        public decimal PcInf { get; set; }

        public bool EsFilaResultado { get; set; }
        public string NombreTotalSoles { get; set; }
        public string NombreTotalDolares { get; set; }

        public int PosRow { get; set; }
        public int PosCol1 { get; set; }
        public int PosCol2 { get; set; }
        public string CeldaExcel { get; set; }

        public int NumeralPadre { get; set; }
        public CbDatosxcentralxfenergDTO CbDatosxcentralxfenerg { get; set; }
        public string Formuladat { get; set; }
        public string FechadatDesc { get; set; }

        public string CcombestadoDesc { get; set; }
    }
}
