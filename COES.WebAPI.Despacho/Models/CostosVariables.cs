using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.WebAPI.Mediciones.Models
{

    /// <summary>
    /// Demanda o Generación cada 30 minutos por día
    /// </summary>  
    public class CostosVariables
    {
        /// <summary>
        /// Modo de operación
        /// </summary>
        public string MODO_OPERACION { get; set; }
        /// <summary>
        /// Escenario
        /// </summary>
        public string ESCENARIO { get; set; }
        /// <summary>
        /// País Perú
        /// </summary>
        public decimal? PE { get; set; }
        public decimal? EFICBTUKWH { get; set; }
        public decimal? EFICTERM { get; set; }
        public decimal? CCOMB { get; set; }
        public decimal? CVC { get; set; }
        public decimal? CVNC { get; set; }
    }
}