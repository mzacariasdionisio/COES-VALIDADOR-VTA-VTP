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
    public class MedicionDTO
    {
        
        /// <summary>
        /// Código del punto de medición
        /// </summary>
        public int Ptomedicodi { get; set; }       
        /// <summary>
        /// Descripción del punto de medición
        /// </summary>
        public string Ptomedidesc { get; set; }             
        /// <summary>
        /// Código del tipo de información
        /// </summary>
        public int? Tipoinfocodi { get; set; }
        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        public string Emprnomb { get; set; }
        
    }
}