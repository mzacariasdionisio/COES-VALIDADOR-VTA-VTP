using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_RESPAGOELE
    /// </summary>
    public class StRespagoeleDTO : EntityBase
    {
        public int Respaecodi { get; set; } 
        public int Respagcodi { get; set; } 
        public int Stcompcodi { get; set; } 
        public string Respaecodelemento { get; set; } 
        public int Respaevalor { get; set; }
        //variables para consulta
        public int Strecacodi { get; set; }
    }
}
