using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_CONFIGURACION
    /// </summary>
    public class SmaConfiguracionDTO : EntityBase
    {
        public int Confsmcorrelativo { get; set; } 
        public string Confsmatributo { get; set; } 
        public string Confsmparametro { get; set; } 
        public string Confsmvalor { get; set; } 
        public string Confsmusucreacion { get; set; } 
        public DateTime? Confsmfeccreacion { get; set; } 
        public string Confsmusumodificacion { get; set; } 
        public DateTime? Confsmfecmodificacion { get; set; } 
        public string Confsmestado { get; set; } 
    }
}
