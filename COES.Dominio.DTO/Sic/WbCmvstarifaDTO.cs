using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_CMVSTARIFA
    /// </summary>
    [Serializable]
    public class WbCmvstarifaDTO : EntityBase
    {
        public int Cmtarcodi { get; set; } 
        public decimal? Cmtarcmprom { get; set; } 
        public decimal? Cmtartarifabarra { get; set; } 
        public decimal? Cmtarprommovil { get; set; } 
        public DateTime? Cmtarfecha { get; set; } 
        public string Cmtarusucreacion { get; set; } 
        public string Cmtarusumodificacion { get; set; } 
        public DateTime? Cmtarfeccreacion { get; set; } 
        public DateTime? Cmtarfecmodificacion { get; set; }

        #region PR5_Informe_Ejecutivo_Semanal
        public int AnioSemana { get; set; }
        public string NombreBarra { get; set; }
        #endregion
    }
}
