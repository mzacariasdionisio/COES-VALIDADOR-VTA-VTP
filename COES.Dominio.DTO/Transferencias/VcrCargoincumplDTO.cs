using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_CARGOINCUMPL
    /// </summary>
    public class VcrCargoincumplDTO : EntityBase
    {
        public int Vcrcicodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Equicodi { get; set; }
        public decimal Vcrcicargoincumplmes { get; set; }
        public decimal Vcrcisaldoanterior { get; set; }
        public decimal Vcrcicargoincumpl { get; set; }
        public decimal Vcrcicarginctransf { get; set; }
        public decimal Vcrcisaldomes { get; set; }
        public decimal VcrcisaldomesAnterior { get; set; }
        public int Pericodidest { get; set; } 
        public string Vcrciusucreacion { get; set; } 
        public DateTime Vcrcifeccreacion { get; set; }
        // ASSETEC 202012
        public decimal Vcrciincumplsrvrsf { get; set; }
        public decimal Vcrciincent { get; set; }
    }
}
