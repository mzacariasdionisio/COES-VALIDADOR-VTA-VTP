using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_POTENCIAFIRME  v2
    /// </summary>
    public class PrPotenciafirmeDTO : EntityBase
    { 
        public int Pfirmecodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Emprcodi { get; set; }
        public DateTime? Pfirmeperiodo { get; set; }
        public decimal? Pfirmevalor { get; set; }
        public string Osinergcodi { get; set; }
        public DateTime? Pfirmefecmodificacion { get; set; }
        public string Pefirmeusumodificacion { get; set; }

        //Propiedades auxiliares
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Unidad { get; set; }
        public int CodCentral { get; set; }
        public decimal Pefectivavalor { get; set; }
        public int? Famcodi { get; set; }
    }

    public class PrPotenciaFirmeReporte : PrPotenciafirmeDTO
    {
        /// <summary>
        /// Potencia firme mes anterior
        /// </summary>
        public decimal? PfirmeAntvalor { get; set; }
        public decimal? Variacion { get; set; }
    }
}