using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_TIPOPUNTOMEDICION
    /// </summary>
    public class MeTipopuntomedicionDTO : EntityBase
    {
        public int? Famcodi { get; set; }
        public string Tipoptomedinomb { get; set; }
        public int Tipoptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }

        public string Fenergnomb { get; set; }
        public string Fenergcolor { get; set; }
    }
}
