using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMERALDAT
    /// </summary>
    public class SpoNumeraldatDTO : EntityBase
    {
        public int Numdatcodi { get; set; } 
        public int Tipoinfocodi { get; set; } 
        public int? Sconcodi { get; set; } 
        public int? Clasicodi { get; set; } 
        public decimal? Numdatvalor { get; set; } 
        public DateTime Numdatfechainicio { get; set; } 
        public DateTime? Numdatfechafin { get; set; }
        public int Verncodi { get; set; }
         
        public string Clasinombre { get; set; }
        public string Sconnomb { get; set; }
        public int? Sconactivo { get; set; }
        public string Numcdescrip { get; set; }
        public int Numecodi { get; set; }
        public int Numccodi { get; set; }
        public int? Sconorden { get; set; }

        public bool TieneCambio { get; set; }

        #region Numerales Datos Base
        public string Emprnomb { get; set; }

        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? Valor { get; set; }

        public string Embalse { get; set; }
        public string Mes { get; set; }
        #endregion
    }
}
