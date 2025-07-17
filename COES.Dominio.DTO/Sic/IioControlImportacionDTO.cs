using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_CONTROL_IMPORTACION
    /// </summary>
    public class IioControlImportacionDTO
    {
        /// <summary>
        /// Identificador de Carga de datos
        /// </summary>
        public int Rcimcodi { get; set; }
        /// <summary>
        /// Periodo al cual pertenece los datos
        /// </summary>
        public int Psiclicodi { get; set; }
        /// <summary>
        /// Código de Tabla
        /// </summary>
        public string Rtabcodi { get; set; }
        /// <summary>
        /// Número de Registros cargados
        /// </summary>
        public int Rcimnroregistros { get; set; }
        /// <summary>
        /// Número de Registros cargados coes
        /// </summary>
        public int Rcimnroregistroscoes { get; set; }
        /// <summary>
        /// Fecha y hora de carga de datos
        /// </summary>
        public DateTime Rcimfechorimportacion { get; set; }
        /// <summary>
        /// Estado de envío de información
        /// </summary>
        public string Rcimestadoimportacion { get; set; }
        /// <summary>
        /// Estado del registro (1=Activo, 0=Inactivo)
        /// </summary>
        public string Rcimestregistro { get; set; }
        /// <summary>
        /// Usuario creador
        /// </summary>
        public string Rcimusucreacion { get; set; }
        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime Rcimfeccreacion { get; set; }
        /// <summary>
        /// Usuario modificador
        /// </summary>
        public string Rcimusumodificacion { get; set; }
        /// <summary>
        /// Fecha y hora de modificación
        /// </summary>
        public DateTime Rcimfecmodificacion { get; set; }
        /// <summary>
        /// Id de envio
        /// </summary>
        public int Enviocodi { get; set; }
        /// <summary>
        /// Empresa que se importo la información
        /// </summary>
        public string Rcimempresa { get; set; }
        /// <summary>
        /// Descripción Empresa que se importo la información
        /// </summary>
        public string Rcimempresadesc { get; set; }
    }
}