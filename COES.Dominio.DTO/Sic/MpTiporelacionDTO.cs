using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_TIPORELACION
    /// </summary>
    public class MpTiporelacionDTO : EntityBase
    {
        public int Mtrelcodi { get; set; } 
        public string Mtrelnomb { get; set; } 
    }
}
