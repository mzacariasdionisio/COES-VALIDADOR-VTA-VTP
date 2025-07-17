using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECPROP
    /// </summary>
    public class FtFictecPropDTO : EntityBase
    {
        public int Ftpropcodi { get; set; }
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
        public string Ftpropnomb { get; set; }
        public int? Ftpropestado { get; set; }
        public string Ftproptipo { get; set; }
        public string Ftpropunidad { get; set; }
        public string Ftpropdefinicion { get; set; }
    }

    public class FtFictecPropiedadValor
    {
        public int Ftpropcodi { get; set; }
        public string Valor { get; set; }
    }
}
