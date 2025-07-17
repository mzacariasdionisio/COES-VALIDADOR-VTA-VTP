using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_TIPOPLANTILLACORREO
    /// </summary>
    public class SiTipoplantillacorreoDTO : EntityBase
    {
        public int Tpcorrcodi { get; set; } 
        public string Tpcorrdescrip { get; set; } 
    }
}
