using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_PERIODO
    /// </summary>
    public class EpoPeriodoDTO : EntityBase
    {
        public int Percodi { get; set; } 
        public int Peranio { get; set; } 
        public int Permes { get; set; } 
        public string Perestado { get; set; } 
    }
}
