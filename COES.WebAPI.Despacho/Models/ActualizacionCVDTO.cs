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
    public class CostosVariablesDTO
    {
        
        /// <summary>
        /// Fecha de los costos variables
        /// </summary>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Tipo de los costos variables
        /// </summary>
        public String Tipo { get; set; }
        /// <summary>
        /// Nombre de los costos variables
        /// </summary>
        public String Nombre { get; set; }
        /// <summary>
        /// Detalle de los costos variables
        /// </summary>
        public String Detalle { get; set; }
        /// <summary>
        /// código de los costos variables
        /// </summary>
        public Int32 Codigo { get; set; }
    }
}