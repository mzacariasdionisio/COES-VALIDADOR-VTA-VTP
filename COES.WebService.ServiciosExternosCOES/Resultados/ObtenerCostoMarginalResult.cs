using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.WebService.ServiciosExternosCOES.Resultados
{
    /// <summary>
    /// Resultado de consulta de Obtener Costo Marginal Programado
    /// </summary>
    public class ObtenerCostoMarginalResult
    {
        /// <summary>
        /// Fecha y hora del costo marginal
        /// </summary>
        public DateTime CMGNFECHA;
        /// <summary>
        /// Componente de energía de Costo Marginal
        /// </summary>
        public decimal? CMGNENERGIA;
        /// <summary>
        /// Componente de Congestión de Costo Marginal
        /// </summary>
        public decimal? CMGNCONGESTION;
        /// <summary>
        /// Costo Marginal Total
        /// </summary>
        public decimal? CMGNTOTAL;
        /// <summary>
        /// Nombre de la barra
        /// </summary>
        public string CNFBARNOMBRE;
    }
}