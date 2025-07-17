using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class SiTipoMensajeDTO : EntityBase
    {
        public int Tmsgcodi { get; set; }
        public string Tmsgnombre { get; set; }
        public string Tmsgcolor { get; set; }
    }
}
