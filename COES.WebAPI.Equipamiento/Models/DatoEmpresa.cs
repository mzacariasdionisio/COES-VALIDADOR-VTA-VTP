using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebAPI.Equipamiento.Models
{
    public class DatoEmpresa
    {
        /// <summary>
        /// Codigo COES de la empresa
        /// </summary>
        public int EMPRCODI { get; set; }

        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        public string EMPRNOMB { get; set; }
        
    }
}