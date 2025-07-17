using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ENVIODETMENSAJE
    /// </summary>
    public class MeEnvioDetMensajeDTO : EntityBase
    {
        public int Edtmsjcodi { get; set; }
        public int? Msgcodi { get; set; }
        public int? Envdetcodi { get; set; }
        public string Edtmsjusucreacion { get; set; }
        public DateTime? Edtmsjfeccreacion { get; set; }
        public string Edtmsjusumodificacion { get; set; }
        public DateTime? Edtmsjfecmodificacion { get; set; } 
    }
}
