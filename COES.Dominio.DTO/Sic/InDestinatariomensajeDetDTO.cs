using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_DESTINATARIOMENSAJE_DET
    /// </summary>
    public class InDestinatariomensajeDetDTO : EntityBase
    {
        public int Evenclasecodi { get; set; }
        public int Indmdecodi { get; set; }
        public int Indemecodi { get; set; }
        public int? Indmdeacceso { get; set; }
    }
}
