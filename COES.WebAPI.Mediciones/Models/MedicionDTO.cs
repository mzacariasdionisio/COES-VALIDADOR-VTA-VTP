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
    public class MedicionDTO 
    {
        
        /// <summary>
        /// Código del punto de medición
        /// </summary>
        public int Ptomedicodi { get; set; }       
        /// <summary>
        /// Nombre del punto de medicion
        /// </summary>
        public string Ptomedielenomb { get; set; }              
        /// <summary>
        /// Descripción del punto de medición
        /// </summary>
        public string Ptomedidesc { get; set; }
        /// <summary>
        /// Código del equipo
        /// </summary>
        public int? Equicodi { get; set; }
        /// <summary>
        /// última fecha modificada
        /// </summary>
        public DateTime? Lastdate { get; set; }


    }
}