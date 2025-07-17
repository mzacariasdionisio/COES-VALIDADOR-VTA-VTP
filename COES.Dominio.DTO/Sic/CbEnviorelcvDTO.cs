using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_ENVIORELCV
    /// </summary>
    public class CbEnviorelcvDTO : EntityBase
    {
        public int Cbcvcodi { get; set; } 
        public int Cbenvcodi { get; set; } 
        public int Repcodi { get; set; } 
    }
}
