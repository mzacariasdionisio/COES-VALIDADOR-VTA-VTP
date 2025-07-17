using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_COMITE_LISTA
    /// </summary>
    public class WbComiteListaDTO : EntityBase
    {
        public int Comitecodi { get; set; }
        public int Comitelistacodi { get; set; }
        public string Comitelistaname { get; set; }
        public string Comitelistaestado { get; set; }
        public string Comitelistausucreacion { get; set; }
        public DateTime? Comitelistafeccreacion { get; set; }

    }
}