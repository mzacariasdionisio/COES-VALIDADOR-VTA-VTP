using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_PERIODOCUADRO
    /// </summary>
    public class IndPeriodocuadroDTO : EntityBase
    {
        public int Percuacodi { get; set; }
        public string Percuatiporigen { get; set; }
        public DateTime? Percuafecini { get; set; }
        public DateTime? Percuafecfin { get; set; }
        public string Percuausumodificacion { get; set; }
        public DateTime Percuafecmodificacion { get; set; }
        public string Percuaflagpotaseg { get; set; }
        public int Descucodi { get; set; }
        public decimal? Percua3factork { get; set; }
        public int? Cuadr3codi { get; set; }
        public int? Percuaanno { get; set; }
        public int? Percuames { get; set; }
        public int Percuaextension { get; set; }
    }
}
