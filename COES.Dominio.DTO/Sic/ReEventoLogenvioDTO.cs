using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_EVENTO_LOGENVIO
    /// </summary>
    public class ReEventoLogenvioDTO : EntityBase
    {
        public int Reevlocodi { get; set; } 
        public int? Reevprcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Reevloindcarga { get; set; } 
        public string Reevlomotivocarga { get; set; } 
        public string Reevlousucreacion { get; set; } 
        public DateTime? Reevlofeccreacion { get; set; } 
        public string Fecha { get; set; }


    }
}
