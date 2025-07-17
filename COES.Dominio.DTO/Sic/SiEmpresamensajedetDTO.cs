using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESAMENSAJEDET
    /// </summary>
    public partial class SiEmpresamensajedetDTO : EntityBase
    {
        public int Emsjdtcodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emsjdtcorreo { get; set; }
        public string Emsjdttipo { get; set; }
        public DateTime? Emsjdtfeclectura { get; set; }
        public string Emsjdtusulectura { get; set; }
        public int Empmsjcodi { get; set; }
    }

    public partial class SiEmpresamensajedetDTO
    {
        public int Msgcodi { get; set; }
    }
}
