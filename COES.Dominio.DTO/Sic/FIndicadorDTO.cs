using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla F_INDICADOR
    /// </summary>
    [Serializable]
    public class FIndicadorDTO : EntityBase
    {
        public DateTime Fechahora { get; set; } 
        public int Gps { get; set; } 
        public string Indiccodi { get; set; } 
        public int Indicitem { get; set; } 
        public decimal? Indicvalor { get; set; }

        #region PR5
        public int Emprcodi { get; set; }
        public string Gpsnomb { get; set; }
        public string HoraFrecMin { get; set; }
        public string HoraFrecMax { get; set; }
        public string HoraTransgr { get; set; }
        public decimal? ValorFrecMin { get; set; }
        public decimal? ValorFrecMax { get; set; }
        public decimal? ValorTransgr { get; set; }
        public string IndicValorTransgr { get; set; }
        public int AcumuladoTransgr { get; set; }
        #endregion

        public string ValorFrecMinDesc { get; set; }
        public string ValorFrecMaxDesc { get; set; }
    }
}
