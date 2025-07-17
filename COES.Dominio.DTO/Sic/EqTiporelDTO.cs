using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_TIPOREL
    /// </summary>
    public class EqTiporelDTO : EntityBase
    {
        public int Tiporelcodi { get; set; } 
        public string Tiporelnomb { get; set; }
        public string TiporelEstado { get; set; }
        public string TiporelUsuarioCreacion { get; set; }
        public DateTime? TiporelFechaCreacion { get; set; }
        public string TiporelUsuarioUpdate { get; set; }
        public DateTime? TiporelFechaUpdate { get; set; }

        public string EstadoDesc { get; set; }

    }

}
