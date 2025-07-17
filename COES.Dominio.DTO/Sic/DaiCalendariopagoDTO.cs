using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DAI_CALENDARIOPAGO
    /// </summary>
    public class DaiCalendariopagoDTO : EntityBase
    {
        public int Calecodi { get; set; } 
        public int Aporcodi { get; set; } 
        public string Caleanio { get; set; } 
        public int? Calenroamortizacion { get; set; } 
        public decimal? Calecapital { get; set; } 
        public decimal? Caleinteres { get; set; } 
        public decimal? Caleamortizacion { get; set; } 
        public decimal? Caletotal { get; set; } 
        public string Calecartapago { get; set; }
        public string Calechequeamortpago { get; set; }
        public string Calechequeintpago { get; set; }
        public string Caleactivo { get; set; } 
        public string Caleusucreacion { get; set; } 
        public DateTime? Calefeccreacion { get; set; } 
        public string Caleusumodificacion { get; set; } 
        public DateTime? Calefecmodificacion { get; set; } 
        public int Tabcdcodiestado { get; set; }

        public string Tabddescripcion { get; set; }
        public string Presanio { get; set; }
    }
}
