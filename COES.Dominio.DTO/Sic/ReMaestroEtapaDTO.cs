using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_MAESTRO_ETAPA
    /// </summary>
    public class ReMaestroEtapaDTO : EntityBase
    {
        public int Reetacodi { get; set; } 
        public string Reetanombre { get; set; } 
        public int? Reetaorden { get; set; } 
        public string Reetaregistro { get; set; } 
        public string Reetausucreacion { get; set; } 
        public DateTime? Reetafeccreacion { get; set; } 
        public string Reetausumodificacion { get; set; } 
        public DateTime? Reetafecmodificacion { get; set; } 
        public DateTime? FechaFinal { get; set; }
        public string Estado { get; set; }


    }
}
