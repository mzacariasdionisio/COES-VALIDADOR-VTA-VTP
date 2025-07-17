using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_DATOSXCENTRALXFENERG
    /// </summary>
    public class CbDatosxcentralxfenergDTO : EntityBase
    {
        public int Cbdatcodi { get; set; } 
        public int Cbcxfecodi { get; set; } 
        public int Ccombcodi { get; set; } 
        public decimal? Cbdatvalor1 { get; set; } 
        public decimal? Cbdatvalor2 { get; set; } 
        public DateTime? Cbdatfecregistro { get; set; } 
        public string Cbdatusuregistro { get; set; } 
    }
}
