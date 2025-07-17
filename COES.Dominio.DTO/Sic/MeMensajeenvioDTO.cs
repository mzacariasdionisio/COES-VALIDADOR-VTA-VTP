using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MENSAJEENVIO
    /// </summary>
    public class MeMensajeenvioDTO : EntityBase
    {
        public int Mencodi { get; set; } 
        public string Menabrev { get; set; } 
        public string Mendescrip { get; set; } 
    }
}
