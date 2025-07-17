using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_REGISTRO_MODPLAN
    /// </summary>
    public class WbRegistroModplanDTO : EntityBase
    {
        public int Regmodcodi { get; set; }
        public string Regmodplan { get; set; }
        public string Regmodversion { get; set; }
        public string Regmodusuario { get; set; }
        public DateTime? Regmodfecha { get; set; }
        public string Emprnomb { get; set; }
        public int Vermplcodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Regmodtipo { get; set; }
        public int Arcmplcodi { get; set; }

    }
}
