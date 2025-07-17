using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_HISTORICO_INDICADOR
    /// </summary>
    public class EpoHistoricoIndicadorDTO : EntityBase
    {
        public int Hincodi { get; set; } 
        public int Indcodi { get; set; } 
        public int Hinanio { get; set; } 
        public decimal Hinmeta { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
    }
}
