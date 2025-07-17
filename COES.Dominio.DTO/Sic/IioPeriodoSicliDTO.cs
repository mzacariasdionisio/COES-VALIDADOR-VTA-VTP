using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_PERIODO_SICLI
    /// </summary>
    public class IioPeriodoSicliDTO
    {
        /// <summary>
        /// Código del Periodo de Remisión
        /// </summary>
        public int PsicliCodi { get; set; }
        /// <summary>
        /// Periodo de Remisión para la Sincronización COES Osinergmin
        /// </summary>
        public string PsicliAnioMesPerrem { get; set; }
        /// <summary>
        /// Ultima fecha de actualización en COES
        /// </summary>
        public DateTime PsicliFecUltActCoes { get; set; }
        /// <summary>
        /// Última fecha de actualización en Osinergmin
        /// </summary>
        public DateTime PsicliFecUltActOsi { get; set; }
        /// <summary>
        /// Estado del Periodo
        /// </summary>
        public string PsicliEstado { get; set; }
        /// <summary>
        /// Estado del registro (1=Activo, 0=Inactivo)
        /// </summary>
        public string PsicliEstRegistro { get; set; }
        /// <summary>
        /// Usuario creador
        /// </summary>
        public string PsicliUsuCreacion { get; set; }
        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime PsicliFecCreacion { get; set; }
        /// <summary>
        /// Usuario modificador
        /// </summary>
        public string PsicliUsuModificacion { get; set; }
        /// <summary>
        /// Fecha y hora de modificación
        /// </summary>
        public DateTime PsicliFecModificacion { get; set; }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Etiqueta del Periodo.
        /// </summary>
        public String EtiquetaPeriodo { get; set; }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        /// <summary>
        /// Indicador si el periodo está abierto o no.
        /// </summary>
        public String PSicliCerrado { get; set; }
        public string PSicliCerradoDemanda { get; set; }

        public DateTime PSicliFecSincronizacion { get; set; }

        public int TablasEmpresasProcesar { get; set; }
    }
}