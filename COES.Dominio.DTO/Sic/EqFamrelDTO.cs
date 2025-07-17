using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_FAMREL
    /// </summary>
    public class EqFamrelDTO : EntityBase
    {
        public int Tiporelcodi { get; set; } 
        public int Famcodi1 { get; set; } 
        public int Famcodi2 { get; set; }
        public int? Famcodi1old { get; set; }
        public int? Famcodi2old { get; set; }
        public int? Famnumconec { get; set; } 
        public string Famreltension { get; set; }
        public string Famrelestado { get; set; }
        public string Famrelusuariocreacion { get; set; }
        public DateTime? Famrelfechacreacion { get; set; }
        public string Famrelusuarioupdate { get; set; }
        public DateTime? Famrelfechaupdate { get; set; }
        public string EstadoDesc { get; set; }

        public string Famnomb1 { get; set; }
        public string Famnomb2 { get; set; }
    }

}
