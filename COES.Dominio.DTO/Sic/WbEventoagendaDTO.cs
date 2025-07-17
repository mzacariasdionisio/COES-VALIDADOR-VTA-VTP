using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_EVENTOAGENDA
    /// </summary>
    public class WbEventoagendaDTO : EntityBase
    {
        public int Eveagcodi { get; set; }
        public int Eveagtipo { get; set; } 
        public DateTime? Eveagfechinicio { get; set; }
        public DateTime? Eveagfechfin { get; set; } 
        public string Eveagubicacion { get; set; } 
        public string Eveagextension { get; set; } 
        public string Eveagusuariocreacion { get; set; } 
        public DateTime? Eveagfechacreacion { get; set; } 
        public string Eveagusuarioupdate { get; set; }
        public DateTime? Eveagfechaupdate { get; set; } 
        public string Eveagtitulo { get; set; } 
        public string Eveagdescripcion { get; set; } 
    }
}
