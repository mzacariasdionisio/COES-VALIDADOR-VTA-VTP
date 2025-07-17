using System;

namespace COES.Dominio.DTO.Sic
{

    public class IioTablaSyncDTO
    {
        /// <summary>
        /// Código de tabla
        /// </summary>
        public string RtabCodi { get; set; }
        /// <summary>
        /// Descripción de Tabla
        /// </summary>
        public string RtabDescripcionTabla { get; set; }
        /// <summary>
        /// Estado de Tabla (Disponible o no)
        /// </summary>
        public string RtabEstadoTabla { get; set; }
        /// <summary>
        /// Código de Tabla oficial
        /// </summary>
        public string RtabCodTablaOsig { get; set; }
        /// <summary>
        /// Estado del registro (1=Activo, 0=Inactivo)
        /// </summary>
        public string RtabEstRegistro { get; set; }
        /// <summary>
        /// Usuario creador
        /// </summary>
        public string RtabUsuCreacion { get; set; }
        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime RtabFecCreacion { get; set; }
        /// <summary>
        /// Usuario modificador
        /// </summary>
        public string RtabUsuModificacion { get; set; }
        /// <summary>
        /// Fecha y hora de modificación
        /// </summary>
        public DateTime RtabFecModificacion { get; set; }

        //- alpha.JDEL - Inicio 09/06/2016: Cambio para atender el requerimiento.
        
        /// <summary>
        /// Query de la tabla
        /// </summary>
        public string RtabQuery { get; set; }

        /// <summary>
        /// Columnas de las tablas
        /// </summary>
        public string RtabNombreTabla { get; set; }
        //- JDEL Fin
        
        //Adicionales
        /// <summary>
        /// Número de Registros cargados
        /// </summary>
        public int RccaNroRegistros { get; set; }
        /// <summary>
        /// Fecha y hora de carga de datos
        /// </summary>
        public DateTime RccaFecHorEnvio { get; set; }
        /// <summary>
        /// Estado de envío de información
        /// </summary>
        public string RccaEstadoEnvio { get; set; }
        /// <summary>
        /// Usuario de que envio la remisión
        /// </summary>
        public string RccaUsuario { get; set; }
    }
}