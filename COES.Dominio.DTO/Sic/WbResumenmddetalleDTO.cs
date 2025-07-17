using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_RESUMENMDDETALLE
    /// </summary>
    public class WbResumenmddetalleDTO : EntityBase
    {
        public int Resmddcodi { get; set; }
        public int Resmdcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Tgenerercodi { get; set; }
        public string Tgenerernomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Tgenerercolor { get; set; }
        public DateTime Resmddfecha { get; set; }
        public decimal Resmddvalor { get; set; }
        public DateTime Resmddmes { get; set; }
        public string Resmddusumodificacion { get; set; }
        public DateTime? Resmddfecmodificacion { get; set; }
        public string Resmddtipogenerrer { get; set; }
    }
}
