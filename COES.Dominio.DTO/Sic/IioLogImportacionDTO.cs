using System;
namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_LOG_IMPORTACION
    /// </summary>
    public class IioLogImportacionDTO
    {
        /// <summary>
        /// Identificador de log de error
        /// </summary>
        public int UlogCodi { get; set; }

        /// <summary>
        /// Periodo Sicli
        /// </summary>
        public string PsicliCodi { get; set; }

        /// <summary>
        /// Usuario de Creación
        /// </summary>
        public string UlogUsuCreacion { get; set; }

        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime UlogFecCreacion { get; set; }

        /// <summary>
        /// Periodo Sicli
        /// </summary>
        public string UlogProceso { get; set; }

        /// <summary>
        /// Tabla Afectada
        /// </summary>
        public string UlogTablaAfectada { get; set; }

        /// <summary>
        /// Línea de error del archivo cargado
        /// </summary>
        public int UlogNroRegistrosAfectados { get; set; }

        /// <summary>
        /// Descripción del error
        /// </summary>
        public string UlogMensaje { get; set; }

        /// <summary>
        /// Identificador de Carga de Importacion
        /// </summary>
        public int RcimCodi { get; set; }

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre de la tabla COES
        /// </summary>
        public string UlogTablaCOES { get; set; }

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Id del registro COES
        /// </summary>
        public string UlogIdRegistroCOES { get; set; }

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Tipo de incidencia
        /// </summary>
        public int UlogTipoIncidencia { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Código CL del Suministro
        /// </summary>
        public string Suministro { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre del Usuario Libre.
        /// </summary>
        public string Cliente { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre de la barra y nivel de tensión
        /// </summary>
        public string Barra { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nivel de tensión de la barra.
        /// </summary>
        public string Tension { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Código del área seleccionada.
        /// </summary>
        public int CodigoArea { get; set; }

        //- alpha.HDT - 11/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Nombre del área seleccionada.
        /// </summary>
        public string Area { get; set; }

        //- alpha.HDT - 20/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Código de la empresa.
        /// </summary>
        public int EmprCodi { get; set; }

        public string Suministrador  { get; set; }
        public int EmprCodiSumi { get; set; }       

    }
}