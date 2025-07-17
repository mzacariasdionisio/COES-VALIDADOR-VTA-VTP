using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECDET
    /// </summary>
    public class FtFictecDetDTO : EntityBase
    {
        public int? Fteccodi { get; set; }
        public int? Fteqcodi { get; set; }
        public int Ftecdcodi { get; set; }
        public DateTime? Ftecdfecha { get; set; }
        public string Ftecdusuario { get; set; }
    }

    public class NotificacionFMDetails
    {
        public int Fteccodi { get; set; }
        public int Fteqcodi { get; set; }
        public string Fteqnombre { get; set; }
        //public string FteqnombreNew { get; set; }
        public string Ftecdusuario { get; set; }
        public DateTime? Ftecdfecha { get; set; }
    }
}
