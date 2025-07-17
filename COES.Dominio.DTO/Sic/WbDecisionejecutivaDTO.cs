using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_DECISIONEJECUTIVA
    /// </summary>
    public class WbDecisionejecutivaDTO : EntityBase
    {
        public int Desejecodi { get; set; } 
        public string Desejedescripcion { get; set; } 
        public DateTime? Desejefechapub { get; set; } 
        public string Desejetipo { get; set; } 
        public string Desejeestado { get; set; }
        public string Desejefile { get; set; }
        public string Desejeextension { get; set; }
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; }
        public List<WbDecisionejecutadoDetDTO> ListaCarta { get; set; }
        public List<WbDecisionejecutadoDetDTO> ListaAntecedentes { get; set; }
        public List<WbDecisionejecutadoDetDTO> ListaItems { get; set; }
        public string FileName { get; set; }
        public string Icono { get; set; }

    }
}
