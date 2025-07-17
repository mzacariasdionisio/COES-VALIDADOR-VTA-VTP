using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_FACTOR_UTILIZACION
    /// </summary>
    public class CoFactorUtilizacionDTO : EntityBase
    {
        public decimal? Facutibeta { get; set; } 
        public string Facutiusucreacion { get; set; } 
        public DateTime? Facutifeccreacion { get; set; } 
        public int Facuticodi { get; set; } 
        public int? Prodiacodi { get; set; } 
        public int? Facutiperiodo { get; set; } 
        public decimal? Facutialfa { get; set; } 
        public DateTime Prodiafecha { get; set; }
        public decimal Perprgvalor { get; set; }
        public string FechaHora { get; set; }
        public string Facutiusumodificacion{ get; set; }
        public DateTime? Facutifecmodificacion { get; set; }
    }

    public class CoFactorUtilizacionFecha
    {
        public DateTime Fecha { get; set; }
        public List<CoFactorUtilizacionDTO> ListaFactorUtilizacion { get; set; }
    }
}
