using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_HISTORICO_INDICADOR_DET
    /// </summary>
    public class EpoHistoricoIndicadorDetDTO : EntityBase
    {
        public int Hincodi { get; set; } 
        public int Percodi { get; set; } 
        public decimal? Hidvalor { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
    }
}
