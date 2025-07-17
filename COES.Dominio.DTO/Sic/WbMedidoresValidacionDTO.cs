using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_MEDIDORES_VALIDACION
    /// </summary>
    public class WbMedidoresValidacionDTO : EntityBase
    {
        public int Medivalcodi { get; set; } 
        public int? Ptomedicodimed { get; set; } 
        public int? Ptomedicodidesp { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Indestado { get; set; }

        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public int Ptomedicodi { get; set; }
        public string Centralmed { get; set; }
        public string Equipomed { get; set; }
        public string Empresamed { get; set; }
        public string Centraldesp { get; set; }
        public string Equipodesp { get; set; }
        public string Empresadesp { get; set; }
        public string Ptomediestado { get; set; }
        public string Ptomediestadomed { get; set; }
        public string Ptomediestadodesp { get; set; }
    }
}
