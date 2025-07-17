using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_BANDEJAMENSAJE_USER
    /// </summary>
    public class SiBandejamensajeUserDTO : EntityBase
    {
        public int Bandcodi { get; set; }
        public int? Modcodi { get; set; }
        public string Bandnombre { get; set; }
        public string Bandusucreacion { get; set; }
        public DateTime? Bandfeccreacion { get; set; }
        public int Cantidad { get; set; }
    }
}
