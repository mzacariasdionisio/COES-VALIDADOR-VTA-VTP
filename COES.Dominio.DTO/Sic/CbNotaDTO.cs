using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_NOTA
    /// </summary>
    public class CbNotaDTO : EntityBase
    {
        public int Cbnotacodi { get; set; } 
        public int Cbrepcodi { get; set; } 
        public string Cbnotaitem { get; set; } 
        public string Cbnotadescripcion { get; set; } 
    }
}
