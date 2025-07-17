using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMCUADRO
    /// </summary>
    public class SpoNumcuadroDTO : EntityBase
    {
        public int Numccodi { get; set; } 
        public int? Numecodi { get; set; } 
        public string Numcdescrip { get; set; } 
        public string Numctitulo { get; set; } 
    }
}
