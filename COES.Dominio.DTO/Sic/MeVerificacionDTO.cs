using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_VERIFICACION
    /// </summary>
    public class MeVerificacionDTO : EntityBase
    {
        public int Verifcodi { get; set; } 
        public string Verifnomb { get; set; } 
    }
}
