using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_PERIODO_SEIN
    /// </summary>
    public class IioPeriodoSeinDTO
    {
        /// <summary>
        /// Id de Periodo de Remisión para la Sincronización COES Osinergmin
        /// </summary>
        public int PseinCodi { get; set; }
        /// <summary>
        /// Periodo de Remisión para la Sincronización COES Osinergmin
        /// </summary>
        public string PseinAnioMesPerrem { get; set; }
        /// <summary>
        /// Fecha de primer envío de información
        /// </summary>
        public DateTime PseinFecPriEnvio { get; set; }
        /// <summary>
        /// Fecha de último envío de información
        /// </summary>
        public DateTime PseinFecUltEnvio { get; set; }
        /// <summary>
        /// Flag confirmación
        /// </summary>
        public string PseinConfirmado { get; set; }
        /// <summary>
        /// Estado del Periodo
        /// </summary>
        public string PseinEstado { get; set; }
        /// <summary>
        /// Estado del registro (1=Activo, 0=Inactivo)
        /// </summary>
        public string PseinEstRegistro { get; set; }
        /// <summary>
        /// Usuario creador
        /// </summary>
        public string PseinUsuCreacion { get; set; }
        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime PseinFecCreacion { get; set; }
        /// <summary>
        /// Usuario modificador
        /// </summary>
        public string PseinUsuModificacion { get; set; }
        /// <summary>
        /// Fecha y hora de modificación
        /// </summary>
        public DateTime PseinFecModificacion { get; set; }

        public string PseinAnioMesPerremDesc { get; set; }
        public string PseinFecPriEnvioDesc { get; set; }
        public string PseinFecUltEnvioDesc { get; set; }
    }
}