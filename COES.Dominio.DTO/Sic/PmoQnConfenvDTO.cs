using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_QN_CONFENV
    /// </summary>
    public class PmoQnConfenvDTO : EntityBase
    {
        public int Qncfgecodi { get; set; }
        public int Qnlectcodi { get; set; }
        public string Qncfgesddps { get; set; }
        public string Qncfgeusucreacion { get; set; }
        public DateTime? Qncfgefeccreacion { get; set; }
    }
}
