namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_LOG_IMPORTACION
    /// </summary>
    public class IioLogRemisionDTO
    {
        /// <summary>
        /// Identificador de log de error
        /// </summary>
        public int RlogCodi { get; set; }
        /// <summary>
        /// Identificador de Carga de datos
        /// </summary>
        public int RccaCodi { get; set; }
        /// <summary>
        /// Línea de error del archivo cargado
        /// </summary>
        public int RlogNroLinea { get; set; }
        /// <summary>
        /// Descripción del error
        /// </summary>
        public string RlogDescripcionError { get; set; }

        /// <summary>
        /// Id de envio
        /// </summary>
        public int Enviocodi { get; set; }
    }
}