using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_CENTRALGEN
    /// </summary>
    public class StCentralgenDTO : EntityBase
    {
        public int Stcntgcodi { get; set; } 
        public int Stgenrcodi { get; set; } 
        public int Equicodi { get; set; } 
        public int Barrcodi { get; set; } 
        public string Stcntgusucreacion { get; set; } 
        public DateTime Stcntgfeccreacion { get; set; } 
        public string Stcntgusumodificacion { get; set; } 
        public DateTime Stcntgfecmodificacion { get; set; }
        //parametro para consulta
        public int Strecacodi { get; set; }
        public string Equinomb { get; set; }
        public string Barrnomb { get; set; }
        //para reportes
        public string Emprnomb { get; set; }
        public decimal Degeledistancia { get; set; }
        public decimal Stenrgrgia { get; set; }
        public decimal Gwhz { get; set; }
        public string Stcompcodelemento { get; set; }

    }
}
