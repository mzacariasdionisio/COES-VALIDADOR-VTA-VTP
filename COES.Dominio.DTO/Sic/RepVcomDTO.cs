using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla REP_VCOM
    /// </summary>
    public class RepVcomDTO : EntityBase
    {
        public int Periodo { get; set; }
        public string Codigomodooperacion { get; set; }
        public string Codigotipocombustible { get; set; }
        public decimal? Valor { get; set; }
    }
}
