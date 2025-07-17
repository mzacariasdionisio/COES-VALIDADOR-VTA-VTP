using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VtdMontoPorPeaje
    /// </summary>
    public class VtdMontoPorPeajeDTO
    {        
        public int Emprcodi { get; set; }    
        public string Emprnomb { get; set; }
        public decimal Valddemandacoincidente { get; set; }
        public decimal Valdpeajeuni { get; set; }
        public DateTime Valofecha { get; set; }
    }
    
}
