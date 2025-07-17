using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_CUADRO
    /// </summary>
    public class PfrCuadroDTO : EntityBase
    {
        public int Pfrcuacodi { get; set; } 
        public string Pfrcuanombre { get; set; } 
        public string Pfrcuatitulo { get; set; } 
        public string Pfrcuasubtitulo { get; set; } 
    }
}
