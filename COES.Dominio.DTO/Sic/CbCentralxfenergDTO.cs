using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_CENTRALXFENERG
    /// </summary>
    public partial class CbCentralxfenergDTO : EntityBase
    {
        public int Cbcxfecodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public int Cbcxfeactivo { get; set; }

        public int Estcomcodi { get; set; }
        public int Cbcxfevisibleapp { get; set; }
        public int Cbcxfenuevo { get; set; }
        public int Cbcxfeexistente { get; set; }
        public int? Cbcxfeorden { get; set; }
    }

    public partial class CbCentralxfenergDTO
    {
        public string Equinomb { get; set; }
        public string Fenergnomb { get; set; }
        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public int Agrupcodi { get; set; }
        public int Catecodi { get; set; }
        public string Equiestado { get; set; }
        public string Tipocentral { get; set; }

        #region region CCGAS.PR31
        public string EstadoDesc { get; set; }
        public string Color { get; set; }
        public decimal? MinPrecioUnitSuministro { get; set; }
        public decimal? MaxPrecioUnitSuministro { get; set; }
        public decimal? MinPrecioUnitTransporte { get; set; }
        public decimal? MaxPrecioUnitTransporte { get; set; }
        public decimal? MinPrecioUnitDistribucion { get; set; }
        public decimal? MaxPrecioUnitDistribucion { get; set; }
        public decimal? MinCostoGasNatural { get; set; }
        public decimal? MaxCostoGasNatural { get; set; }
        #endregion
    }
}
