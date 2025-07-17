using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebAPI.Equipamiento.Models
{
    public class DatoFamilia
    {
        /// <summary>
        /// Codigo COES de la Familia (Tipo de equipos)
        /// </summary>
        public int FAMCODI { get; set; }
        /// <summary>
        /// Abreviatura de la Familia (Tipo de equipos)
        /// </summary>
        public string FAMABREV { get; set; }
        /// <summary>
        /// Nombre de la Familia (Tipo de equipos)
        /// </summary>
        public string FAMNOMB { get; set; }

    }
}