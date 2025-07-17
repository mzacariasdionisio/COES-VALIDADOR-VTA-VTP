using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.WebAPI.Hidrologia.Models
{

    /// <summary>
    /// Demanda o Generación cada 30 minutos por día
    /// </summary>  
    public class Medicion_DTO
    {
        
        /// <summary>
        /// Fecha de la medición
        /// </summary>
        public DateTime Medifecha { get; set; }       
        /// <summary>
        /// Descripción del punto de medición
        /// </summary>
        public string Ptomedidesc { get; set; }             
        /// <summary>
        /// Descripción del tipo de información
        /// </summary>
        public int? Tipoinfodesc { get; set; }
        /// <summary>
        /// Valor del H1
        /// </summary>
        public decimal? H1 { get; set; }
        
    }
}