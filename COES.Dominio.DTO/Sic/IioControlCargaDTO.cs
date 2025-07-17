using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_CONTROL_CARGA
    /// </summary>
    public class IioControlCargaDTO
    {
        /// <summary>
        /// Identificador de Carga de datos
        /// </summary>
        public int RccaCodi { get; set; }
        /// <summary>
        /// Periodo al cual pertenece los datos
        /// </summary>
        public int PseinCodi { get; set; }
        /// <summary>
        /// Código de Tabla
        /// </summary>
        public string RtabCodi { get; set; }
        /// <summary>
        /// Número de Registros cargados
        /// </summary>
        public int RccaNroRegistros { get; set; }
        /// <summary>
        /// Fecha y hora de carga de datos
        /// </summary>
        public DateTime? RccaFecHorEnvio { get; set; }
        /// <summary>
        /// Estado de envío de información
        /// </summary>
        public string RccaEstadoEnvio { get; set; }
        /// <summary>
        /// Estado del registro (1=Activo, 0=Inactivo)
        /// </summary>
        public string RccaEstRegistro { get; set; }
        /// <summary>
        /// Usuario creador
        /// </summary>
        public string RccaUsuCreacion { get; set; }
        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime? RccaFecCreacion { get; set; }
        /// <summary>
        /// Usuario modificador
        /// </summary>
        public string RccaUsuModificacion { get; set; }
        /// <summary>
        /// Fecha y hora de modificación
        /// </summary>
        public DateTime? RccaFecModificacion { get; set; }

        /// <summary>
        /// Id de envio
        /// </summary>
        public int Enviocodi { get; set; }
        
        public string RccaEstadoEnvioDesc { get; set; }
        public string RccaFecCreacionDesc { get; set; }
        public string RccaFecModificacionDesc { get; set; }
    }
}