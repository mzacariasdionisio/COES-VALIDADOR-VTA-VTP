using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_TRANSMISOR
    /// </summary>
    public class StTransmisorDTO : EntityBase
    {
        public int Stranscodi { get; set; } 
        public int Strecacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public string Stransusucreacion { get; set; } 
        public DateTime Stransfeccreacion { get; set; }

        public string Emprnomb { get; set; }
    }
}
