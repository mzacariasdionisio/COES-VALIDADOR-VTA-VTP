using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace COES.WebAPI.Mediciones.Models
{
    /// <summary>
    /// 
    /// </summary>
    
    public class OrdenamientoMD
    {
        /// <summary>
        /// Valor de Máxima demanda
        /// </summary>
        public decimal? MaximaDemanda { get; set; }
        /// <summary>
        /// Generación total MW
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Valor de Importación (negativo) o Exportación (positivo) en MW
        /// </summary>
        public decimal ValorInternacional { get; set; }
        /// <summary>
        /// Fecha y hora
        /// </summary>        
        public string FechaHora { get; set; }
    }
    
}