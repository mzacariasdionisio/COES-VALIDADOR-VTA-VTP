using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INFORME
    /// </summary>
    public class EveInformeDTO : EntityBase
    {
        public int Evencodi { get; set; } 
        public int Eveninfcodi { get; set; } 
        public int Emprcodi { get; set; } 
        public string Infestado { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Infversion { get; set; } 
        public string Indestado { get; set; } 
        public string Indplazo { get; set; }
        public string Emprnomb { get; set; }
        public string Lastuserrev { get; set; }
        public DateTime? Lastdaterev { get; set; }        
    }

    public class EventoInformeReporte
    {
        public int Emprcodi { get; set; }
        public int Evencodi { get; set; }
        public string Emprnomb { get; set; }
        public string EstadoPreliminar { get; set; }
        public string EstadoFinal { get; set; }        
        public string EstadoComplementario { get; set; }
        public string EstadoPreliminarInicial { get; set; }
    }
}
