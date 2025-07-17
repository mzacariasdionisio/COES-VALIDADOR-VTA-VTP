using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_COMPMENSUALELE
    /// </summary>
    public class StCompmensualeleDTO : EntityBase
    {
        public int Cmpmelcodi { get; set; } 
        public int Cmpmencodi { get; set; } 
        public int Stcompcodi { get; set; } 
        public string Cmpmelcodelemento { get; set; } 
        public decimal Cmpmelvalor { get; set; } 

        //para consulta
        public string Equinomb { get; set; }
        //public string Stcompcodelemento { get; set; }
    }
}
