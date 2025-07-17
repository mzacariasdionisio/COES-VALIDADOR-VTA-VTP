using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_INDICADOR
    /// </summary>
    public class EpoIndicadorDTO : EntityBase
    {
        public int Indcodi { get; set; } 
        public string Indnomb { get; set; } 
        public string Indmensajeleyenda { get; set; } 
        public string Indformatoescalavalores { get; set; } 
        public decimal Indintervalo { get; set; } 
    }
}
