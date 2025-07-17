using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_COSTOMARGINALPROG
    /// </summary>
    public class CmCostomarginalprogDTO : EntityBase
    {
        public int Cmarprcodi { get; set; } 
        public int Cnfbarcodi { get; set; } 
        public decimal? Cmarprtotal { get; set; } 
        public DateTime? Cmarprfecha { get; set; } 
        public string Cmarprlastuser { get; set; } 
        public DateTime? Cmarprlastdate { get; set; } 

        #region MonitoreoMME
        public int Grupocodi { get; set; }
        #endregion

        #region SIOSEIN
        public string Osinergcodi { get; set; }
        public string Cnfbarnombre { get; set; }
        #endregion

        #region IMME

        public string Fechadia { get; set; }
        public int Totalregdia { get; set; }

        #endregion
    }
}
