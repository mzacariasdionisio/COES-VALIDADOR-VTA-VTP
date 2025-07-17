using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.WebAPI.Reserva.Models
{

    /// <summary>
    /// Demanda o Generación cada 30 minutos por día
    /// </summary>  
    public class CostosVariables
    {
        /// <summary>
        /// URS
        /// </summary>
        public string URS { get; set; }
        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        public string Empresa { get; set; }
        /// <summary>
        /// Nombre de la central
        /// </summary>
        public string Central { get; set; }
        /// <summary>
        /// Nombre del tipo de equipo
        /// </summary>
        public string TipoEquipo { get; set; }
        /// <summary>
        /// Nombre del equipo
        /// </summary>
        public string Equipo { get; set; }
        /// <summary>
        /// Tipo
        /// </summary>
        public string Tipo { get; set; }
        /// <summary>
        /// Inicio de la fecha
        /// </summary>
        public DateTime FechaHoraInicio { get; set; }
        /// <summary>
        /// Fin de la fecha
        /// </summary>
        public DateTime FechaHoraFin { get; set; }
        /// <summary>
        /// Valor de la reserva
        /// </summary>
        public Decimal ValorReserva { get; set; }
        /// <summary>
        /// Hora
        /// </summary>
        public string Hora { get; set; }
    }
}